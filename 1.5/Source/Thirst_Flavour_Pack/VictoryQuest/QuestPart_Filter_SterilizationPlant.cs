using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Filter_SterilizationPlant(int count = 3) : QuestPart_Filter
{
    public int Count = count;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Count, "Count");
    }

    protected override bool Pass(SignalArgs args)
    {
        return WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt >= Count;
    }
}
