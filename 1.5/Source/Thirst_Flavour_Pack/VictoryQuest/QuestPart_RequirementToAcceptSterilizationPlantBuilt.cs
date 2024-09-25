using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptSterilizationPlantBuilt(int required = 3) : QuestPart_RequirementsToAccept
{
    public int Required = required;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref Required, "Required");
    }
    public override AcceptanceReport CanAccept()
    {
        return WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt < Required ? new AcceptanceReport("MSS_Thirst_QuestSterilizationPlantRequired".Translate(WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt, Required)) : true;
    }
}
