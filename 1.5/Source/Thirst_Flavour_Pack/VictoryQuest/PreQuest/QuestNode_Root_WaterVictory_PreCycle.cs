using System;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.PreQuest;

public class QuestNode_Root_WaterVictory_PreCycle: QuestNode
{
    public static string QuestSignal = "FirstPowerRegulatorBuilt";

    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;

      string letterReminder = QuestGen.GenerateNewSignal("SendLetterReminder");
      QuestGen.GenerateNewSignal("ActivateLetterReminderSignal");
      quest.AddPart(new QuestPart_RequirementToAcceptPowerRegulatorBuilt(1));

      QuestPartActivable part1 = new QuestPart_CheckPowerRegulator(1);
      part1.inSignalEnable = quest.AddedSignal;
      part1.outSignalsCompleted.Add(QuestSignal);
      part1.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      quest.AddPart(part1);

      QuestPart_PassOutInterval part2 = new QuestPart_PassOutInterval();
      part2.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      part2.inSignalEnable = QuestSignal;
      part2.ticksInterval = new IntRange(0, 0);
      part2.outSignals.Add(letterReminder);
      quest.AddPart(part2);

      QuestPart_Filter part3 = new QuestPart_Filter_PowerRegulator(1);
      part3.inSignal = letterReminder;
      part3.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted");
      part3.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      quest.AddPart(part3);
      quest.CanAcceptQuest((Action) (() =>
      {
          QuestNode_ResolveQuestName.Resolve();
          string resolvedQuestName = slate.Get<string>("resolvedQuestName");
          LetterDef positiveEvent = LetterDefOf.PositiveEvent;
          string label = "LetterLabelArchonexusWealthReached".Translate((NamedArgument) resolvedQuestName);
          string text = "MSS_Thirst_LetterTextReqReached".Translate((NamedArgument) resolvedQuestName);
          quest.Letter(positiveEvent, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, text: text, label: label);
      }), inSignal: part3.outSignal, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly);

      quest.RewardChoice().choices.Add(new QuestPart_Choice.Choice
      {
        rewards = {
          new Reward_WaterInfo
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
