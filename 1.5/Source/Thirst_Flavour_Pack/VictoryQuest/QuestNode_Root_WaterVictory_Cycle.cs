using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public abstract class QuestNode_Root_WaterVictory_Cycle: QuestNode
{
    public const int LetterReminderInterval = 3600000;
    protected Map map;

    protected abstract int WaterCycle { get; }
    protected abstract string QuestSignal { get; }
    protected abstract QuestPart_RequirementsToAccept Requirement { get; }
    protected abstract QuestPartActivable Part1 { get; }
    protected abstract QuestPart_Filter Part3 { get; }


    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;
      this.map = QuestGen_Get.GetMap();

      string newSignal2 = QuestGen.GenerateNewSignal("SendLetterReminder");
      QuestGen.GenerateNewSignal("ActivateLetterReminderSignal");
      quest.AddPart(Requirement);

      QuestPartActivable part1 = Part1;
      part1.inSignalEnable = quest.AddedSignal;
      part1.outSignalsCompleted.Add(QuestSignal);
      part1.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      quest.AddPart(part1);

      QuestPart_PassOutInterval part2 = new QuestPart_PassOutInterval();
      part2.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      part2.inSignalEnable = QuestSignal;
      part2.ticksInterval = new IntRange(3600000, 3600000);
      part2.outSignals.Add(newSignal2);
      quest.AddPart(part2);

      QuestPart_Filter part3 = Part3;
      part3.inSignal = newSignal2;
      part3.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted");
      part3.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      quest.AddPart(part3);
      quest.CanAcceptQuest((Action) (() =>
      {
        QuestNode_ResolveQuestName.Resolve();
        string str1 = slate.Get<string>("resolvedQuestName");
        Quest quest1 = quest;
        LetterDef positiveEvent = LetterDefOf.PositiveEvent;
        string str2 = "LetterLabelArchonexusWealthReached".Translate((NamedArgument) str1);
        string text = "MSS_Thirst_LetterTextReqReached".Translate((NamedArgument) str1);
        string label = str2;
        quest1.Letter(positiveEvent, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, text: text, label: label);
      }), inSignal: part3.outSignal, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly);

      quest.RewardChoice().choices.Add(new QuestPart_Choice.Choice
      {
        rewards = {
          new Reward_WaterMap
          {
              currentPart = WaterCycle
          }
        }
      });
      List<MapParent> var = new List<MapParent>();
      List<Map> maps = Find.Maps;
      for (int index = 0; index < maps.Count; ++index)
      {
        Map map = maps[index];
        if (map.IsPlayerHome)
          var.Add(map.Parent);
      }
      slate.Set("playerSettlements", var);
      slate.Set("playerSettlementsCount", var.Count);
      slate.Set("colonistsAllowed", 5);
      slate.Set("animalsAllowed", 5);
      slate.Set("requiredWealth", 350000f);
      slate.Set("map", this.map);
      slate.Set("mapParent", this.map.Parent);
      slate.Set("studyRequirement", false);
    }

    protected void PickNewColony(
        WorldObjectDef worldObjectDef,
        int maxRelics = 1)
    {
        Quest quest = QuestGen.quest;
        Slate slate = QuestGen.slate;
        string newSignal1 = QuestGen.GenerateNewSignal("NewColonyCreated");
        string newSignal2 = QuestGen.GenerateNewSignal("NewColonyCancelled");
        quest.AddPart(new QuestPart_NewColony
        {
            inSignal = slate.Get<string>("inSignal"),
            outSignalCompleted = newSignal1,
            outSignalCancelled = newSignal2,
            worldObjectDef = worldObjectDef,
            maxRelics = maxRelics
        });
        quest.SetQuestNotYetAccepted(newSignal2, true);
        quest.End(QuestEndOutcome.Success, inSignal: newSignal1);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;
}
