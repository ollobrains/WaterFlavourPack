using System;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Thirst_Flavour_Pack.VictoryQuest.Rewards;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_PreCycle: QuestNode
{
    public static string QuestSignal = "FirstPowerRegulatorBuilt";

    private static float ThreatPointsFactor = 0.6f;

    protected override void RunInt()
    {
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;

      QuestGen.GenerateNewSignal("ActivateLetterReminderSignal");

      quest.RewardChoice().choices.Add(new QuestPart_Choice.Choice
      {
        rewards = {
          new Reward_ArhcospringBuildingSiteMap()
        }
      });

      float num1 = slate.Get<float>("points");
      Faction faction = slate.Get<Faction>("roughTribe");
      int tile;
      TryFindSiteTile(out tile);
      if (faction != null)
          quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
      quest.DialogWithCloseBehavior("[questDescriptionBeforeAccepted]", inSignal: quest.AddedSignal, signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly, closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd);
      quest.DescriptionPart("[questDescriptionBeforeAccepted]", quest.AddedSignal, quest.InitiateSignal, QuestPart.SignalListenMode.OngoingOrNotYetAccepted);
      quest.DescriptionPart("[questDescriptionAfterAccepted]", quest.InitiateSignal, signalListenMode: QuestPart.SignalListenMode.OngoingOrNotYetAccepted);
      quest.Letter(LetterDefOf.PositiveEvent, text: "[questAcceptedLetterText]", label: "[questAcceptedLetterLabel]");
      float num2 = Find.Storyteller.difficulty.allowViolentQuests ? num1 * ThreatPointsFactor : 0.0f;
      SitePartParams parms = new SitePartParams
      {
          threatPoints = num2
      };
      Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_PowerRegulator_Site, parms)), tile, Faction.OfAncients);
      if (num1 <= 0.0 && Find.Storyteller.difficulty.allowViolentQuests)
          quest.SetSitePartThreatPointsToCurrent(site, Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_PowerRegulator_Site, QuestGen_Get.GetMap().Parent, threatPointsFactor: ThreatPointsFactor);
      quest.SpawnWorldObject(site);
      slate.Set("factionless", faction == null);
      slate.Set("threatsEnabled", Find.Storyteller.difficulty.allowViolentQuests);

    }

    private bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
    {
        return TileFinder.TryFindNewSiteTile(out tile, 10, 40, exitOnFirstTileFound: exitOnFirstTileFound);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;
}
