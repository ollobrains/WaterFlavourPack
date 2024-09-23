using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Filter_CatalyticSeparator : QuestPart_Filter
{
    protected override bool Pass(SignalArgs args)
    {
        return WaterVictoryWorldComponent.Instance.CatalyticSeparatorsBuilt >= 3;
    }
}
