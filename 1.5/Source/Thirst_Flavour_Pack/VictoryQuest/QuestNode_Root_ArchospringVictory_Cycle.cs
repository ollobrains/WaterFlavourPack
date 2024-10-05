using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Thirst_Flavour_Pack.VictoryQuest.Rewards;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public abstract class QuestNode_Root_ArchospringVictory_Cycle: QuestNode
{
    public const int LetterReminderInterval = 3600000;
    protected Map map;

    protected abstract int WaterCycle { get; }
    protected abstract string QuestSignal { get; }
    protected abstract QuestPart_Filter QuestPartFilter { get; }
    protected abstract QuestPart_RequirementToAcceptBuildingHasComponents Requirement { get; }

    public string subquestsCompletedSignal;

    protected abstract bool SetSuccess { get; }


    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;
      map = QuestGen_Get.GetMap();

      slate.Set("archospringComponent", Thirst_Flavour_PackDefOf.MSS_Water_ComponentArcho);
      string componentLostSignal = QuestGen.GenerateNewSignal("ComponentLost");
      subquestsCompletedSignal = QuestGen.GenerateNewSignal("SubquestsCompleted");
      quest.End(QuestEndOutcome.Fail, inSignal: componentLostSignal, sendStandardLetter: true);

      quest.AddPart(Requirement);

      QuestPart_SubquestGenerator_ArchoHunt part1 = new QuestPart_SubquestGenerator_ArchoHunt();
      part1.componentLostSignal = componentLostSignal;
      part1.inSignalEnable = QuestGen.slate.Get<string>("inSignal");
      part1.interval = new IntRange(600, 600);
      part1.archotechComponentDef = Thirst_Flavour_PackDefOf.MSS_Water_ComponentArcho;
      part1.archotechComponentSlateName = "archotechComponent";
      part1.useMapParentThreatPoints = map?.Parent;
      part1.expiryInfoPartKey = "RelicInfoFound";
      part1.maxSuccessfulSubquests = 1;
      part1.subquestDefs.AddRange(GetAllSubquests(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory));

      part1.signalListenMode = QuestPart.SignalListenMode.NotYetAcceptedOnly;
      quest.AddPart(part1);

      QuestPart_Filter filter = QuestPartFilter;
      filter.signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
      filter.inSignal = QuestGen.slate.Get<string>("inSignal");
      filter.outSignal = QuestGen.GenerateNewSignal("FilterComplete");

      quest.AddPart(filter);

      if (SetSuccess)
          quest.End(QuestEndOutcome.Success, inSignal:filter.outSignal, sendStandardLetter: true);

      QuestPart_Choice choice = quest.RewardChoice();
      choice.inSignalChoiceUsed = subquestsCompletedSignal;
      choice.choices.Add(new QuestPart_Choice.Choice
      {
        rewards = {
          new Reward_ArchospringMap
          {
              currentPart = WaterCycle
          }
        }
      });
      List<Map> maps = Find.Maps;

      List<MapParent> var = (from m in maps where m.IsPlayerHome select m.Parent).ToList();

      slate.Set("inSignal", subquestsCompletedSignal);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;

    public IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
    {
        return DefDatabase<QuestScriptDef>.AllDefs.Where(q => q.epicParent == parent);
    }
}
