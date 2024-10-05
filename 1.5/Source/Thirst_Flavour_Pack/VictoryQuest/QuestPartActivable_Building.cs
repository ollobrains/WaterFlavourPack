using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPartActivable_Building(ThingDef buildingDef): QuestPartActivable
{
    public ThingDef BuildingDef = buildingDef;

    public override void QuestPartTick()
    {
        var val = Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingAvailable.GetWithFallback(BuildingDef, false);

        if (Find.TickManager.TicksGame % 60 != 0 || !val)
            return;
        Complete();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref BuildingDef, "BuildingDef");
    }
}
