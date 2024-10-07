using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class SitePartWorker_BarbCamp: SitePartWorker
{
    public Thing component;
    public static string ComponentObtainedSignal = "ArchospringComponentObtained";

    public bool componentObtainedSignalSent = false;

    public override void Init(Site site, SitePart sitePart)
    {
        base.Init(site, sitePart);

        ThingDef compDef = QuestGen.slate.Get<ThingDef>("MSS_Thirst_ArchospringComponent");
        if (compDef == null) compDef = Thirst_Flavour_PackDefOf.MSS_Thirst_ComponentArcho;
        sitePart.things = new ThingOwner<Thing>(sitePart);

        component = ThingMaker.MakeThing(compDef).MakeMinified();
        sitePart.things.TryAdd(component);
    }

    public override void SitePartWorkerTick(SitePart sitePart)
    {
        if (!componentObtainedSignalSent && component.holdingOwner != null && component.holdingOwner.Owner is Pawn_InventoryTracker pawnInv && pawnInv.pawn.Faction == Faction.OfPlayer)
        {
            QuestUtility.SendQuestTargetSignals(sitePart.site.questTags, ComponentObtainedSignal, component.LabelCap);
            componentObtainedSignalSent = true;
        }
    }
}
