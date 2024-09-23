using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_WaterVictory_ThirdCycle: QuestNode_Root_WaterVictory_Cycle
{
    protected override int ArchonexusCycle => 3;
    protected override string QuestSignal => "SterilizationPlantBuilt";
    protected override QuestPart_RequirementsToAccept Requirement => new QuestPart_RequirementToAcceptSterilizationPlantBuilt();
    protected override QuestPartActivable Part1 => new QuestPart_SterilizationPlant();
    protected override QuestPart_Filter Part3 => new QuestPart_Filter_SterilizationPlant();

    private static float ThreatPointsFactor = 0.6f;

    protected override void RunInt()
    {
      base.RunInt();
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;
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
      Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(SitePartDefOf.Archonexus, parms)), tile, Faction.OfAncients);
      if (num1 <= 0.0 && Find.Storyteller.difficulty.allowViolentQuests)
        quest.SetSitePartThreatPointsToCurrent(site, SitePartDefOf.Archonexus, map.Parent, threatPointsFactor: ThreatPointsFactor);
      quest.SpawnWorldObject(site);
      slate.Set("factionless", faction == null);
      slate.Set("threatsEnabled", Find.Storyteller.difficulty.allowViolentQuests);
    }

    private bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
    {
      return TileFinder.TryFindNewSiteTile(out tile, 10, 40, exitOnFirstTileFound: exitOnFirstTileFound);
    }

    protected override bool TestRunInt(Slate slate)
    {
      return base.TestRunInt(slate) && TryFindSiteTile(out int _, true);
    }
}
