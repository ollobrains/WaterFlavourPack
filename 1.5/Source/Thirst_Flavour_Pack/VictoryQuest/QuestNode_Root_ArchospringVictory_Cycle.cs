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
    protected Map map;

    // Signal name for tracking destruction of the archo buildings in order to reset quests
    public static string BuildingDestroyedGlobalSignal = "MSS_Thirst_ArchoBuildingDestroyed";

    // The current part of the quest cycles
    protected abstract int QuestCycle { get; }

    // The signal that's raised when the building is complete
    public static string ArchoBuildingMapVisitedSignalForDef(ThingDef def)
    {
        return $"{def.defName}_MapVisited";
    }

    // The signal that's raised when the building is complete
    public static string ArchoBuildingCompleteSignalForDef(ThingDef def)
    {
        return $"{def.defName}_ArchoBuildingComplete";
    }

    // A filter on the global building destroyed signal to determine if the building we're interested in was destroyed, and if it is, raises it's own signal
    // This lets us act on our specific building being destroyed, without having to worry about raising specific signals
    protected abstract QuestPartActivable_BuildingUnavailable BuildingFilter { get; }

    // The def of the relevant building, e.g. Power Regulator
    protected abstract ThingDef BuildingDef { get; }

    // The SitePartDef we should use to create the quest site
    protected abstract SitePartDef CurrentSitePartDef { get; }

    // Should we spawn the site
    protected abstract bool SpawnSite { get; }

    // Should we trigger the success condition (used for the third part)
    protected abstract bool SetSuccess { get; }


    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;
      map = QuestGen_Get.GetMap();

      // pre-create any signals we need
      string mapVisitedSignal = ArchoBuildingMapVisitedSignalForDef(BuildingDef); // global, so don't use QuestGen.GenerateNewSignal
      string bldCompleteSignal = ArchoBuildingCompleteSignalForDef(BuildingDef);

      // Local signal
      string bldDestroyedSignal = QuestGen.GenerateNewSignal("Building_Destroyed");
      string questOver = QuestGen.GenerateNewSignal("QuestOver");

      if (SpawnSite)
      {
          TryFindSiteTile(out int tile);

          SitePartParams parms = new SitePartParams();
          Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(CurrentSitePartDef, parms)), tile, Faction.OfAncients);

          //Don't spawn the site until we accept.
          quest.SpawnWorldObject(site, null, quest.InitiateSignal);

          //Don't send the letter until we accept.
          quest.Letter(
              LetterDefOf.RelicHuntInstallationFound,
              text: "MSS_Thirst_Letter_Site_Found_Title".Translate(CurrentSitePartDef.label),
              label: "MSS_Thirst_Letter_Site_Found_Text".Translate(CurrentSitePartDef.label),
              inSignal:quest.InitiateSignal);
      }

      // Set the description based on quest accepted/not accepted
      quest.DescriptionPart("[questDescriptionBeforeAccepted]", quest.AddedSignal, mapVisitedSignal, QuestPart.SignalListenMode.OngoingOrNotYetAccepted);
      quest.DescriptionPart("[questDescriptionAfterAccepted]", mapVisitedSignal, signalListenMode: QuestPart.SignalListenMode.OngoingOrNotYetAccepted);


      // Intercept the building destroyed signal, and check if it applies to us. If so, reset the quest to not accepted so we can try again.
      QuestPartActivable_BuildingUnavailable filter = BuildingFilter;
      filter.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
      filter.inSignalEnable = QuestGen.GenerateNewSignal(BuildingDestroyedGlobalSignal);
      filter.outSignalsCompleted.Add(bldDestroyedSignal);
      quest.SetQuestNotYetAccepted(bldDestroyedSignal);

      // Spawn subquests at random intervals that can give components
      QuestPart_SubquestGenerator_ArchoHunt part1 = new QuestPart_SubquestGenerator_ArchoHunt();
      part1.inSignalEnable = mapVisitedSignal;
      part1.inSignalDisable = bldCompleteSignal;
      part1.interval = Thirst_Flavour_PackMod.settings.ArchoQuestComponentHuntInterval;
      part1.archotechComponentDef = Thirst_Flavour_PackDefOf.MSS_Thirst_ComponentArcho;
      part1.archotechComponentSlateName = "MSS_Thirst_ArchospringComponent";
      part1.useMapParentThreatPoints = map?.Parent;
      part1.expiryInfoPartKey = "RelicInfoFound";
      part1.maxSuccessfulSubquests = 3;
      part1.subquestDefs.AddRange(GetAllSubquests(Thirst_Flavour_PackDefOf.MSS_Thirst_EndGame_WaterVictory));
      part1.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
      quest.AddPart(part1);

      // End the quest here if appropriate
      if (SetSuccess)
      {
          quest.SignalPass(inSignal:bldCompleteSignal, outSignal:questOver);
          quest.End(QuestEndOutcome.Success, inSignal: questOver, sendStandardLetter: true);
      }

      // set up the "reward"
      QuestPart_Choice choice = quest.RewardChoice();
      choice.inSignalChoiceUsed = bldCompleteSignal;
      choice.choices.Add(new QuestPart_Choice.Choice
      {
        rewards = {
          new Reward_ArchospringMap
          {
              currentPart = QuestCycle
          }
        }
      });

      slate.Set("inSignal", bldCompleteSignal);
    }


    public static bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
    {
        return TileFinder.TryFindNewSiteTile(out tile, 10, 40, exitOnFirstTileFound: exitOnFirstTileFound);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;

    public static IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
    {
        return DefDatabase<QuestScriptDef>.AllDefs.Where(q => q.epicParent == parent);
    }
}
