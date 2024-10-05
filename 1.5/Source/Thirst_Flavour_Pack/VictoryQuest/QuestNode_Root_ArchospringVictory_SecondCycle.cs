using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_SecondCycle: QuestNode_Root_ArchospringVictory_Cycle
{
    protected override int WaterCycle => 2;
    protected override string QuestSignal => "CatalyticSeparatorBuilt";
    protected override QuestPart_Filter QuestPartFilter => new QuestPart_Filter_ArchoSpringBuilding(Thirst_Flavour_PackDefOf.MSS_CatalyticSeparator, 3);

    protected override QuestPart_RequirementToAcceptBuildingHasComponents Requirement =>
        new QuestPart_RequirementToAcceptBuildingHasComponents(Thirst_Flavour_PackDefOf.MSS_CatalyticSeparator);

    protected override bool SetSuccess => true;
    protected override void RunInt()
    {
        base.RunInt();
        Quest quest = QuestGen.quest;
        quest.DialogWithCloseBehavior("[resolvedQuestDescription]", inSignal: quest.AddedSignal, signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd);
    }
}
