using RimWorld;
using RimWorld.QuestGen;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_WaterVictory_FirstCycle: QuestNode_Root_WaterVictory_Cycle
{
    protected override int WaterCycle => 1;
    protected override string QuestSignal => "PowerRegulatorBuilt";
    protected override QuestPart_RequirementsToAccept Requirement => new QuestPart_RequirementToAcceptPowerRegulatorBuilt();
    protected override QuestPart_Filter QuestPartFilter => new QuestPart_Filter_PowerRegulator();

    protected override void RunInt()
    {
        base.RunInt();
        PickNewColony(WorldObjectDefOf.Settlement_SecondArchonexusCycle);
    }
}
