using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptPowerRegulatorBuilt: QuestPart_RequirementsToAccept
{
    public override AcceptanceReport CanAccept()
    {
        return WaterVictoryWorldComponent.Instance.PowerRegulatorsBuilt < 1 ? new AcceptanceReport("MSS_Thirst_QuestPowerRegRequired".Translate()) : (AcceptanceReport) true;
    }
}
