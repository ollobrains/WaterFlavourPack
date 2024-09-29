using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_FirstCycle: QuestNode_Root_ArchospringVictory_Cycle
{
    protected override int WaterCycle => 1;
    protected override string QuestSignal => "PowerRegulatorBuilt";
    protected override QuestPart_RequirementsToAccept Requirement => new QuestPart_RequirementToAcceptPowerRegulatorBuilt();
    protected override QuestPart_Filter QuestPartFilter => new QuestPart_Filter_PowerRegulator();
    protected override ThingDef ArchotechComponent => Thirst_Flavour_PackDefOf.MSS_PowerRegulator;

    protected override void RunInt()
    {
        base.RunInt();
        PickNewColony(Thirst_Flavour_PackDefOf.MSS_Settlement_SecondArchospringCycle);
    }
}
