using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Filter_SterilizationPlant : QuestPart_Filter
{
    protected override bool Pass(SignalArgs args)
    {
        return WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt >= 3;
    }
}
