using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class Building_ArchoMachine : Building,
    IThingHolderEvents<Thing_ComponentArcho>,
    IHaulEnroute,
    IHaulDestination,
    IHaulSource,
    ISearchableContents
{
    private ThingOwner<Thing_ComponentArcho> innerContainer;
    private StorageSettings settings;

    public ThingOwner SearchableContents => innerContainer;

    public void GetChildHolders(List<IThingHolder> outChildren)
    {
        ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
    }

    public ThingOwner GetDirectlyHeldThings() => innerContainer;

    public void Notify_SettingsChanged()
    {
        if (!Spawned)
            return;
        MapHeld.listerHaulables.Notify_HaulSourceChanged(this);
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
        return (innerContainer.InnerListForReading.Count < 3 || innerContainer.InnerListForReading.Contains(t)) && GetStoreSettings().AllowedToAccept(t) && innerContainer.CanAcceptAnyOf(t);
    }

    public int SpaceRemainingFor(ThingDef _) => 3 - innerContainer.InnerListForReading.Count;

    public void Notify_ItemAdded(Thing_ComponentArcho item)
    {
        MapHeld.listerHaulables.Notify_AddedThing(item);
        Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingComponentCount.SetOrAdd(def, innerContainer.Count);
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
        foreach (FloatMenuOption floatMenuOption in HaulSourceUtility.GetFloatMenuOptions(this, selPawn))
            yield return floatMenuOption;

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

        sb.Append("MSS_Thirst_BuildingContainsComponents".Translate(innerContainer.Count));

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

        if(innerContainer.Count < 3)
            Find.SignalManager.SendSignal(new Signal(QuestNode_Root_ArchospringVictory_PreCycle.PowerRegulatorDestroyed, global: true));

        Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingAvailable.SetOrAdd(def, false);
    }
}
