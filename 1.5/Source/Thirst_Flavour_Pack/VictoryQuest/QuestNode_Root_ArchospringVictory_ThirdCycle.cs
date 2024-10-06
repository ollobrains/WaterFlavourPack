using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_ThirdCycle: QuestNode_Root_ArchospringVictory_Cycle
{
    protected override int QuestCycle => 3;
    protected override QuestPart_Activable_ArchoSpringBuilding Activable_ArchoSpringBuilding => new QuestPart_Activable_ArchoSpringBuilding(Thirst_Flavour_PackDefOf.MSS_SterilizationPlant, 3);

    protected override SitePartDef CurrentSitePartDef => Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_SterilizationPlant_Site;
    protected override QuestPartActivable_BuildingUnavailable BuildingFilter => new QuestPartActivable_BuildingUnavailable(Thirst_Flavour_PackDefOf.MSS_SterilizationPlant);
    protected override ThingDef BuildingDef => Thirst_Flavour_PackDefOf.MSS_SterilizationPlant;

    private static float ThreatPointsFactor = 0.6f;
    protected override bool SpawnSite => true;
    protected override bool SetSuccess => false;

    protected override void RunInt()
    {
      base.RunInt();
      Quest quest = QuestGen.quest;
      Slate slate = QuestGen.slate;

      // Fire up the dialog and a letter
      quest.DialogWithCloseBehavior(
          "[questDescriptionBeforeAccepted]",
          inSignal: quest.AddedSignal,
          signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly,
          closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd);

      quest.Letter(LetterDefOf.PositiveEvent, text: "[questAcceptedLetterText]", label: "[questAcceptedLetterLabel]");


      // grab a free tile for the site
      int tile;
      TryFindSiteTile(out tile);

      float raidPoints = slate.Get<float>("points");
      float adjustedRaidPoints = Find.Storyteller.difficulty.allowViolentQuests ? raidPoints * ThreatPointsFactor : 0.0f;
      SitePartParams parms = new SitePartParams
      {
        threatPoints = adjustedRaidPoints
      };
      Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(Thirst_Flavour_PackDefOf.MSS_Thirst_ArchospringSite, parms)), tile, Faction.OfAncients);
      if (raidPoints <= 0.0 && Find.Storyteller.difficulty.allowViolentQuests)
        quest.SetSitePartThreatPointsToCurrent(site, Thirst_Flavour_PackDefOf.MSS_Thirst_ArchospringSite, map.Parent, threatPointsFactor: ThreatPointsFactor);
      quest.SpawnWorldObject(site);

      slate.Set("factionless", true);
      slate.Set("threatsEnabled", Find.Storyteller.difficulty.allowViolentQuests);
    }

    protected override bool TestRunInt(Slate slate)
    {
      return base.TestRunInt(slate) && TryFindSiteTile(out int _, true);
    }
}
