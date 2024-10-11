using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Makes the archo buildings thing holders with appropriate jobs/actions so that we can put archo components in
/// </summary>
public class Building_ArchoMachine : Building,
    IThingHolderEvents<Thing_ComponentArcho>,
    IHaulEnroute,
    IHaulSource,
    IHaulDestination,
    ISearchableContents
{
    private ThingOwner<Thing_ComponentArcho> innerContainer;
    private StorageSettings settings;

    public ThingOwner SearchableContents => null;

    public void GetChildHolders(List<IThingHolder> outChildren)
    {
        ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
    }

    public bool IsComplete()
    {
        return innerContainer.Count >= Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding;
    }

    public ThingOwner GetDirectlyHeldThings() => innerContainer;

    public void Notify_SettingsChanged()
    {
    }

    public bool StorageTabVisible => false;

    public StorageSettings GetStoreSettings()
    {
        return settings;
    }

    public StorageSettings GetParentStoreSettings()
    {
        return def.building.fixedStorageSettings;
    }

    public bool Accepts(Thing t)
    {
        return (innerContainer.InnerListForReading.Count < Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding || innerContainer.InnerListForReading.Contains(t)) && GetStoreSettings().AllowedToAccept(t) && innerContainer.CanAcceptAnyOf(t);
    }

    public int SpaceRemainingFor(ThingDef _) => Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding - innerContainer.InnerListForReading.Count;

    public void Notify_ItemAdded(Thing_ComponentArcho item)
    {
        MapHeld.listerHaulables.Notify_AddedThing(item);
        ModLog.Debug($"Building_ArchoMachine.Notify_ItemAdded: {item.LabelCap}. Current Count: {innerContainer.InnerListForReading.Count}");

        if(innerContainer.Count >= Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding)
            Find.SignalManager.SendSignal(new Signal(QuestNode_Root_ArchospringVictory_Cycle.ArchoBuildingCompleteSignalForDef(def), new SignalArgs(new NamedArgument(this, "building")), true));
    }

    public void Notify_ItemRemoved(Thing_ComponentArcho item)
    {
    }

    public Building_ArchoMachine()
    {
        innerContainer = new ThingOwner<Thing_ComponentArcho>(this, false);
    }

    public override void PostMake()
    {
        base.PostMake();
        settings = new StorageSettings(this);
        if (def.building.defaultStorageSettings != null)
            settings.CopyFrom(def.building.defaultStorageSettings);
    }

    public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
    {
        innerContainer.TryDropAll(Position, Map, ThingPlaceMode.Near);
        base.DeSpawn(mode);
    }

    public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
    {
        foreach (Pawn pawn in Map.PlayerPawnsForStoryteller.Where(p=>p.inventory.innerContainer.Contains(Thirst_Flavour_PackDefOf.MSS_Thirst_ComponentArcho)))
        {
            foreach (Thing component in pawn.inventory.innerContainer.Where(thing => thing.def == Thirst_Flavour_PackDefOf.MSS_Thirst_ComponentArcho))
            {
                string reason = "MSS_Thirst_UnknownReasonForNoHaul";

                bool canHaul = true;

                if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
                {
                    canHaul = false;
                    reason = "MSS_Thirst_IncapableOfManipulation";
                }else if (!pawn.CanReach(this, PathEndMode.ClosestTouch, pawn.NormalMaxDanger()))
                {
                    canHaul = false;
                    reason = "MSS_Thirst_CannotReach";
                }

                void Action()
                {
                    Job job = HaulAIUtility.HaulToContainerJob(pawn, component, this);
                    pawn.jobs.StartJob(job);
                }

                yield return new FloatMenuOption(canHaul ? "MSS_Thirst_HaulComponentToArchoBuilding".Translate(pawn.Name.ToStringShort) : "MSS_Thirst_CannotHaulComponentToArchoBuilding".Translate(pawn.Name.ToStringShort, reason.Translate()), canHaul ? Action : null);
            }

        }

        foreach (Thing_ComponentArcho component in (IEnumerable<Thing_ComponentArcho>) innerContainer.InnerListForReading)
        {
            foreach (FloatMenuOption floatMenuOption in component.GetFloatMenuOptions(selPawn))
                yield return floatMenuOption;
        }

        foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
            yield return floatMenuOption;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
        Scribe_Deep.Look(ref settings, "settings", this);
    }

    public override string GetInspectString()
    {
        StringBuilder sb = new StringBuilder(base.GetInspectString());

        sb.Append("MSS_Thirst_BuildingContainsComponents".Translate(innerContainer.Count, Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding));

        return sb.ToString();
    }

    public override void SetFaction(Faction newFaction, Pawn recruiter = null)
    {
        base.SetFaction(newFaction, recruiter);

        Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingAvailable.SetOrAdd(def, true);
    }

    public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
    {
        base.Destroy(mode);

        Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingAvailable.SetOrAdd(def, false);

        if(innerContainer.Count < Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding)
            Find.SignalManager.SendSignal(new Signal(QuestNode_Root_ArchospringVictory_Cycle.BuildingDestroyedGlobalSignal, new SignalArgs(new NamedArgument(def, "buildingDef")), global: true));
    }
}
