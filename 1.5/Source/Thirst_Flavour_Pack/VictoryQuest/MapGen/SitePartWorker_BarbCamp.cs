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

    public int NextCheck = 60;

    public override void Init(Site site, SitePart sitePart)
    {
        base.Init(site, sitePart);

        ThingDef compDef = QuestGen.slate.Get<ThingDef>("MSS_Thirst_ArchospringComponent") ?? Thirst_Flavour_PackDefOf.MSS_Thirst_ComponentArcho;
        sitePart.things = new ThingOwner<Thing>(sitePart);

        component = ThingMaker.MakeThing(compDef).MakeMinified();
        sitePart.things.TryAdd(component);
    }

    public override void SitePartWorkerTick(SitePart sitePart)
    {
        if (Find.TickManager.TicksGame > NextCheck)
        {
            NextCheck = Find.TickManager.TicksGame + 60;
            if (!componentObtainedSignalSent && sitePart.site.Map != null && !sitePart.site.Map.mapPawns.AllHumanlike.Any(p=>p.Faction.HostileTo(Faction.OfPlayer)))
            {
                QuestUtility.SendQuestTargetSignals(sitePart.site.questTags, ComponentObtainedSignal);
                componentObtainedSignalSent = true;
            }
        }
    }
}
