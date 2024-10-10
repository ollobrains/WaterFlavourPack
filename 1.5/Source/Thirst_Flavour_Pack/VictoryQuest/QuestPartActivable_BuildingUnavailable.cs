using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Once enabled, checks if the building is available every 60 ticks. If it is, complete and send our output signals
/// </summary>
/// <param name="buildingDef"></param>
public class QuestPartActivable_BuildingUnavailable(ThingDef buildingDef): QuestPartActivable
{
    public ThingDef BuildingDef = buildingDef;
    public int NextCheck = 600;

    public override void QuestPartTick()
    {
        if (Find.TickManager.TicksGame < NextCheck)
            return;

        NextCheck += 600;

        if (Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingAvailable.GetWithFallback(BuildingDef, false))
        {
            ModLog.Debug($"{BuildingDef} is now available, proceeding with quest.");
            Complete();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref BuildingDef, "BuildingDef");
        Scribe_Values.Look(ref NextCheck, "NextCheck");
    }
}
