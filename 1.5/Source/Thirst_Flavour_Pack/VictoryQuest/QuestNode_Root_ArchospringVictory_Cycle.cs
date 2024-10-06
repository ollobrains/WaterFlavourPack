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
    protected Map map;

    public static string BuildingDestroyedGlobalSignal = "ArchoBuildingDestroyed";

    protected abstract int WaterCycle { get; }
    protected abstract string QuestSignal { get; }
    protected abstract QuestPart_Activable_ArchoSpringBuilding Activable_ArchoSpringBuilding { get; }
    protected abstract QuestPart_RequirementToAcceptBuildingHasComponents Requirement { get; }
    protected abstract QuestPartActivable_BuildingUnavailable BuildingFilter { get; }
    protected abstract ThingDef BuildingDef { get; }

    protected abstract SitePartDef CurrentSitePartDef { get; }

    protected abstract bool SpawnSite { get; }
    protected abstract bool SetSuccess { get; }


    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;
      map = QuestGen_Get.GetMap();

      string activated = QuestGen.GenerateNewSignal("ArchoBuildingComplete");
      string mapVisitedSignal = BuildingDef.defName + "_MapVisited";

      if (SpawnSite)
      {
          int tile;
          TryFindSiteTile(out tile);

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
      string bldDestroyedSignal = QuestGen.GenerateNewSignal("Building_Destroyed");
      QuestPartActivable_BuildingUnavailable filter = BuildingFilter;
      filter.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
      filter.inSignalEnable = QuestGen.GenerateNewSignal(BuildingDestroyedGlobalSignal);
      filter.outSignalsCompleted.Add(bldDestroyedSignal);
      quest.SetQuestNotYetAccepted(bldDestroyedSignal);

      // quest.AddPart(Requirement);

      // Spawn subquests at random intervals that can give components
      QuestPart_SubquestGenerator_ArchoHunt part1 = new QuestPart_SubquestGenerator_ArchoHunt();
      part1.inSignalEnable = mapVisitedSignal;
      part1.inSignalDisable = activated;
      part1.interval = new IntRange(600, 600);
      part1.archotechComponentDef = Thirst_Flavour_PackDefOf.MSS_Water_ComponentArcho;
      part1.archotechComponentSlateName = "archotechComponent";
      part1.useMapParentThreatPoints = map?.Parent;
      part1.expiryInfoPartKey = "RelicInfoFound";
      part1.maxSuccessfulSubquests = 1;
      part1.subquestDefs.AddRange(GetAllSubquests(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory));

      part1.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
      quest.AddPart(part1);

      // Check if the 3 components are in place, then fire the ArchoBuildingComplete signal
      QuestPart_Activable_ArchoSpringBuilding buildingComplete = Activable_ArchoSpringBuilding;
      buildingComplete.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
      // always listen, just in case the player beats us to adding the components before accepting
      buildingComplete.inSignalEnable = mapVisitedSignal;
      buildingComplete.outSignalsCompleted.Add(activated);
      quest.AddPart(buildingComplete);

      // End the quest here if appropriate
      if (SetSuccess)
          quest.End(QuestEndOutcome.Success, inSignal:activated, sendStandardLetter: true);

      // set up the "reward"
      QuestPart_Choice choice = quest.RewardChoice();
      choice.inSignalChoiceUsed = activated;
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

      slate.Set("inSignal", activated);
    }


    public bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
    {
        return TileFinder.TryFindNewSiteTile(out tile, 10, 40, exitOnFirstTileFound: exitOnFirstTileFound);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;

    public IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
    {
        return DefDatabase<QuestScriptDef>.AllDefs.Where(q => q.epicParent == parent);
    }
}
