using RimWorld;
using RimWorld.QuestGen;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_WaterVictory_SecondCycle: QuestNode_Root_WaterVictory_Cycle
{
    protected override int WaterCycle => 2;
    protected override string QuestSignal => "CatalyticSeparatorBuilt";
    protected override QuestPart_RequirementsToAccept Requirement => new QuestPart_RequirementToAcceptCatalyticSeparatorBuilt();
    protected override QuestPart_Filter QuestPartFilter => new QuestPart_Filter_CatalyticSeparator();
    protected override void RunInt()
    {
        base.RunInt();
        Quest quest = QuestGen.quest;
        quest.DialogWithCloseBehavior("[resolvedQuestDescription]", inSignal: quest.AddedSignal, signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd);

        PickNewColony(Thirst_Flavour_PackDefOf.MSS_Settlement_ThirdArchospringCycle, 2);
    }
}
