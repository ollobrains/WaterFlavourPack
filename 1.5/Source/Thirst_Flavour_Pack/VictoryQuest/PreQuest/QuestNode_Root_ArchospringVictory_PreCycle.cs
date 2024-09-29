using System;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.PreQuest;

public class QuestNode_Root_ArchospringVictory_PreCycle: QuestNode
{
    public static string QuestSignal = "FirstPowerRegulatorBuilt";

    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;

      string letterReminder = QuestGen.GenerateNewSignal("SendLetterReminder");
      QuestGen.GenerateNewSignal("ActivateLetterReminderSignal");
      quest.AddPart(new QuestPart_RequirementToAcceptPowerRegulatorBuilt(1));

      QuestPart_Filter filter = new QuestPart_Filter_PowerRegulator(1);
      filter.inSignal = letterReminder;
      filter.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted");
      filter.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      quest.AddPart(filter);
      quest.CanAcceptQuest((Action) (() =>
      {
          QuestNode_ResolveQuestName.Resolve();
          string resolvedQuestName = slate.Get<string>("resolvedQuestName");
          LetterDef positiveEvent = LetterDefOf.PositiveEvent;
          string label = "LetterLabelArchonexusWealthReached".Translate((NamedArgument) resolvedQuestName);
          string text = "MSS_Thirst_LetterTextReqReached".Translate((NamedArgument) resolvedQuestName);
          quest.Letter(positiveEvent, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, text: text, label: label);
      }), inSignal: filter.outSignal, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly);

      quest.RewardChoice().choices.Add(new QuestPart_Choice.Choice
      {
        rewards = {
          new Reward_ArchospringInfo
          {
              currentPart = 1
          }
        }
      });

      QuestPart_QuestEnd endPart = new QuestPart_QuestEnd();
      endPart.inSignal = QuestGen.GenerateNewSignal("Initiate");
      endPart.sendLetter = true;
      endPart.playSound = true;
      endPart.outcome = QuestEndOutcome.Success;

      quest.AddPart(endPart);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;
}
