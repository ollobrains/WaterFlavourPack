using RimWorld;
using RimWorld.QuestGen;
using Thirst_Flavour_Pack.VictoryQuest.Rewards;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_PreCycle : QuestNode
{ protected override void RunInt()
    {
        ModLog.Debug("QuestNode_Root_ArchospringVictory_PreCycle.RunInt");
        Quest quest = QuestGen.quest;

        quest.RewardChoice().choices.Add(new QuestPart_Choice.Choice { rewards = { new Reward_ArhcospringBuildingSiteMap() } });

        quest.DialogWithCloseBehavior("[questDescription]", inSignal: quest.AddedSignal, signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly,
            closeAction: QuestPartDialogCloseAction.CloseActionKey.None);
        quest.Letter(LetterDefOf.PositiveEvent, text: "[questAcceptedLetterText]", label: "[questAcceptedLetterLabel]");

        quest.End(QuestEndOutcome.Success, inSignal: quest.InitiateSignal);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;
}
