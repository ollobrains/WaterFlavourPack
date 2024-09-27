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
    protected abstract QuestPart_Filter QuestPartFilter { get; }


    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;
      map = QuestGen_Get.GetMap();

      string newSignal2 = QuestGen.GenerateNewSignal("SendLetterReminder");
      QuestGen.GenerateNewSignal("ActivateLetterReminderSignal");
      quest.AddPart(Requirement);

      QuestPart_Filter filter = QuestPartFilter;
      filter.inSignal = newSignal2;
      filter.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted");
      filter.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      quest.AddPart(filter);
      quest.CanAcceptQuest((Action) (() =>
      {
        QuestNode_ResolveQuestName.Resolve();
        string resolvedQuestName = slate.Get<string>("resolvedQuestName");
        LetterDef positiveEvent = LetterDefOf.PositiveEvent;
        string label = "LetterLabelArchonexusWealthReached".Translate((NamedArgument) resolvedQuestName);
        string desc = "MSS_Thirst_LetterTextReqReached".Translate((NamedArgument) resolvedQuestName);
        quest.Letter(positiveEvent, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, text: desc, label: label);
      }), inSignal: filter.outSignal, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly);

      quest.RewardChoice().choices.Add(new QuestPart_Choice.Choice
      {
        rewards = {
          new Reward_WaterMap
          {
              currentPart = WaterCycle
          }
        }
      });
      List<Map> maps = Find.Maps;

      List<MapParent> var = (from m in maps where m.IsPlayerHome select m.Parent).ToList();

      slate.Set("playerSettlements", var);
      slate.Set("playerSettlementsCount", var.Count);
      slate.Set("colonistsAllowed", Thirst_Flavour_PackMod.settings.WaterQuestColonistsAllowed);
      slate.Set("animalsAllowed", Thirst_Flavour_PackMod.settings.WaterQuestColonistsAllowed);
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
        quest.AddPart(new QuestPart_NewColonyWater
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
