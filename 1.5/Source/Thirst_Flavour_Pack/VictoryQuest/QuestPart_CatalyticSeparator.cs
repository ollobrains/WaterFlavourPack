using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_CatalyticSeparator : QuestPartActivable
{
    public override void QuestPartTick()
    {
        if(Find.TickManager.TicksGame % 60 != 0 || WaterVictoryWorldComponent.Instance.CatalyticSeparatorsBuilt < 3)
            return;
        Complete();
    }
}
