using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptCatalyticSeparatorBuilt: QuestPart_RequirementsToAccept
{
    public override AcceptanceReport CanAccept()
    {
        return WaterVictoryWorldComponent.Instance.CatalyticSeparatorsBuilt < 3 ? new AcceptanceReport("MSS_Thirst_QuestCatalyticSeparatorRequired".Translate(WaterVictoryWorldComponent.Instance.CatalyticSeparatorsBuilt, 3)) : true;
    }
}
