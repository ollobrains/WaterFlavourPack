using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Activable_ArchoSpringBuilding( ThingDef def, int count = 3) : QuestPartActivable
{
    public ThingDef Def = def;
    public int Count = count;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Count, "Count");
        Scribe_Defs.Look(ref Def, "Def");
    }

    public override void QuestPartTick()
    {
        if (Find.TickManager.TicksGame % 60 != 0 || Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingComponentCount.GetWithFallback(Def, 0) < Count)
            return;
        Complete();
    }
}
