using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public abstract class QuestNode_Root_ArchospringVictory_Cycle: QuestNode
{
    public const int LetterReminderInterval = 3600000;
    protected Map map;

    protected abstract int WaterCycle { get; }
    protected abstract string QuestSignal { get; }
    protected abstract QuestPart_RequirementsToAccept Requirement { get; }
    protected abstract QuestPart_Filter QuestPartFilter { get; }
    protected abstract ThingDef ArchotechComponent { get; }

    public string subquestsCompletedSignal;

    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;
      map = QuestGen_Get.GetMap();

      // string buildingObtainedSignal = QuestGen.GenerateNewSignal("buildingObtained");
      // string newSignal2 = QuestGen.GenerateNewSignal("SendLetterReminder");
      // QuestGen.GenerateNewSignal("ActivateLetterReminderSignal");

      // quest.AddPart(Requirement);

      // QuestPart_Filter filter = QuestPartFilter;
      // filter.outSignal = QuestGen.GenerateNewSignal("OuterNodeCompleted");
      // quest.AddPart(filter);

      // string questAcceptedSignal = QuestGen.GenerateNewSignal("QuestAccepted");
      //
      // quest.CanAcceptQuest((Action) (() =>
      // {
      //   QuestNode_ResolveQuestName.Resolve();
      //   string resolvedQuestName = slate.Get<string>("resolvedQuestName");
      //   LetterDef positiveEvent = LetterDefOf.PositiveEvent;
      //   string label = "LetterLabelArchonexusWealthReached".Translate((NamedArgument) resolvedQuestName);
      //   string desc = "MSS_Thirst_LetterTextReqReached".Translate((NamedArgument) resolvedQuestName);
      //   quest.Letter(positiveEvent, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, text: desc, label: label);
      // }), inSignal: filter.outSignal, signalListenMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, outSignal:questAcceptedSignal);

      slate.Set("ArchospringComponent", ArchotechComponent);
      string componentLostdSignal = QuestGen.GenerateNewSignal("ComponentLost");
      subquestsCompletedSignal = QuestGen.GenerateNewSignal("SubquestsCompleted");
      quest.End(QuestEndOutcome.Fail, inSignal: componentLostdSignal, sendStandardLetter: true);

      QuestPart_SubquestGenerator_ArchoHunt part1 = new QuestPart_SubquestGenerator_ArchoHunt();
      part1.componentLostSignal = componentLostdSignal;
      part1.inSignalEnable = QuestGen.slate.Get<string>("inSignal");
      part1.interval = new IntRange(60, 600);
      part1.archotechComponentDef = ArchotechComponent;
      part1.archotechComponentSlateName = "archotechComponent";
      part1.useMapParentThreatPoints = map?.Parent;
      part1.expiryInfoPartKey = "RelicInfoFound";
      part1.maxSuccessfulSubquests = 1;
      part1.subquestDefs.AddRange(GetAllSubquests(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory));
      // part1.outSignalsCompleted.Add(subquestsCompletedSignal);
      part1.signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
      quest.AddPart(part1);

      // quest.SignalPass(inSignal: QuestSignal, outSignal: buildingObtainedSignal);

      var choice = quest.RewardChoice();
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

      // Thing buildingThing = GenerateBuildingComponent(Building);
      //
      // float x1 = Find.Storyteller.difficulty.allowViolentQuests ? slate.Get<float>("points") : 0.0f;
      // SitePartParams parms = new SitePartParams
      // {
      //     points = x1,
      //     relicThing = buildingThing,
      //     relicLostSignal = componentLostdSignal
      // };
      //
      // int tile;
      // TileFinder.TryFindNewSiteTile(out tile, 2, 10, exitOnFirstTileFound: false);
      //
      // Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(SitePartDefOf.AncientAltar, parms)), tile, Faction.OfAncientsHostile);

      List<MapParent> var = (from m in maps where m.IsPlayerHome select m.Parent).ToList();

      slate.Set("inSignal", subquestsCompletedSignal);
      slate.Set("playerSettlements", var);
      slate.Set("playerSettlementsCount", var.Count);
      slate.Set("colonistsAllowed", Thirst_Flavour_PackMod.settings.WaterQuestColonistsAllowed);
      slate.Set("animalsAllowed", Thirst_Flavour_PackMod.settings.WaterQuestColonistsAllowed);
      slate.Set("map", map);
      slate.Set("mapParent", map.Parent);
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
        quest.AddPart(new QuestPart_NewColony_ArchospringVictory
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

    public IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
    {
        return DefDatabase<QuestScriptDef>.AllDefs.Where(q => q.epicParent == parent);
    }
}
