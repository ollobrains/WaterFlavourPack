using RimWorld;
using RimWorld.QuestGen;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_WaterVictory_FirstCycle: QuestNode_Root_WaterVictory_Cycle
{
    protected override int ArchonexusCycle => 1;
    protected override string QuestSignal => "PowerRegulatorBuilt";
    protected override QuestPart_RequirementsToAccept Requirement => new QuestPart_RequirementToAcceptPowerRegulatorBuilt();
    protected override QuestPartActivable Part1 => new QuestPart_PowerRegulator();
    protected override QuestPart_Filter Part3 => new QuestPart_Filter_PowerRegulator();

    protected override void RunInt()
    {
        base.RunInt();
        PickNewColony(WorldObjectDefOf.Settlement_SecondArchonexusCycle);
    }
}
