using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptPowerRegulatorBuilt(int required = 3) : QuestPart_RequirementsToAccept
{
    public int Required = required;

    public override AcceptanceReport CanAccept()
    {
        return WaterVictoryWorldComponent.Instance.PowerRegulatorsBuilt < Required ? new AcceptanceReport("MSS_Thirst_QuestPowerRegRequired".Translate(WaterVictoryWorldComponent.Instance.PowerRegulatorsBuilt, Required)) : true;
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref Required, "Required");
    }
}
