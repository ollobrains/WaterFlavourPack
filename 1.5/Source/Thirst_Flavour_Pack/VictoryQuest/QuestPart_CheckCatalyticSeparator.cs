using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_CheckCatalyticSeparator : QuestPartActivable
{
    public int count = 0;

    public QuestPart_CheckCatalyticSeparator(int count = 3)
    {
        this.count = count;
    }
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref count, "count");
    }
    public override void QuestPartTick()
    {
        if(Find.TickManager.TicksGame % 60 != 0 || WaterVictoryWorldComponent.Instance.CatalyticSeparatorsBuilt < count)
            return;
        Complete();
    }
}
