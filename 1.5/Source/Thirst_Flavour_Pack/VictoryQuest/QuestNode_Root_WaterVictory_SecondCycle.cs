using RimWorld;
using RimWorld.QuestGen;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_WaterVictory_SecondCycle: QuestNode_Root_WaterVictory_Cycle
{
    protected override int WaterCycle => 2;
    protected override string QuestSignal => "CatalyticSeparatorBuilt";
    protected override QuestPart_RequirementsToAccept Requirement => new QuestPart_RequirementToAcceptCatalyticSeparatorBuilt();
    protected override QuestPartActivable Part1 => new QuestPart_CatalyticSeparator();
    protected override QuestPart_Filter Part3 => new QuestPart_Filter_CatalyticSeparator();
    protected override void RunInt()
    {
        base.RunInt();
        Quest quest = QuestGen.quest;
        quest.DialogWithCloseBehavior("[resolvedQuestDescription]", inSignal: quest.AddedSignal, signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd);

        PickNewColony(WorldObjectDefOf.Settlement_ThirdArchonexusCycle, 2);
    }
}
