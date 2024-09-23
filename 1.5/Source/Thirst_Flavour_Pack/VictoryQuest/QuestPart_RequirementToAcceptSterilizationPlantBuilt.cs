using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptSterilizationPlantBuilt: QuestPart_RequirementsToAccept
{
    public override AcceptanceReport CanAccept()
    {
        return WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt < 1 ? new AcceptanceReport("MSS_Thirst_QuestSterilizationPlantRequired".Translate(WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt, 3)) : true;
    }
}
