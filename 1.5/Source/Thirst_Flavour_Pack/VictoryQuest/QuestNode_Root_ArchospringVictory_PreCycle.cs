using System;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Thirst_Flavour_Pack.VictoryQuest.Rewards;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_PreCycle : QuestNode
{
    public string QuestSignal = "PowerRegulatorSeen";
    public static string PowerRegulatorDestroyed = "PowerRegulatorDestroyed";

    protected override void RunInt()
    {
        Quest quest = QuestGen.quest;
        Slate slate = QuestGen.slate;

        string questSignal = QuestGen.GenerateNewSignal(QuestSignal);
        string powerRegulatorDestroyed = QuestGen.GenerateNewSignal(PowerRegulatorDestroyed);

        quest.RewardChoice().choices.Add(new QuestPart_Choice.Choice { rewards = { new Reward_ArhcospringBuildingSiteMap() } });

        string SuccessSignal = QuestGen.GenerateNewSignal("pawnCanAccessRegulator");

        Faction faction = slate.Get<Faction>("roughTribe");
        int tile;
        TryFindSiteTile(out tile);
        if (faction != null)
            quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
        quest.DialogWithCloseBehavior("[questDescriptionBeforeAccepted]", inSignal: quest.AddedSignal, signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly,
            closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd);
        quest.DescriptionPart("[questDescriptionBeforeAccepted]", quest.AddedSignal, quest.InitiateSignal, QuestPart.SignalListenMode.OngoingOrNotYetAccepted);
        quest.DescriptionPart("[questDescriptionAfterAccepted]", quest.InitiateSignal, signalListenMode: QuestPart.SignalListenMode.OngoingOrNotYetAccepted);
        quest.Letter(LetterDefOf.PositiveEvent, text: "[questAcceptedLetterText]", label: "[questAcceptedLetterLabel]");

        SitePartParams parms = new SitePartParams();
        Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_PowerRegulator_Site, parms)), tile,
            Faction.OfAncients);

        quest.SpawnWorldObject(site);
        slate.Set("factionless", faction == null);
        slate.Set("threatsEnabled", false);

        // string signal = QuestGen.GenerateNewSignal("timer");
        // QuestPart_PassOutInterval timerTrigger = new QuestPart_PassOutInterval();
        // timerTrigger.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
        // timerTrigger.ticksInterval = new IntRange(60, 600);
        // timerTrigger.outSignals.Add(signal);
        // quest.AddPart(timerTrigger);

        QuestPartActivable_Building hasAccessToPowerReg = new QuestPartActivable_Building(Thirst_Flavour_PackDefOf.MSS_PowerRegulator)
        {
            inSignalEnable = quest.AddedSignal, signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted
        };
        hasAccessToPowerReg.outSignalsCompleted.Add(questSignal);

        quest.AddPart(hasAccessToPowerReg);

        quest.SetQuestNotYetAccepted(powerRegulatorDestroyed, true);
        quest.End(QuestEndOutcome.Success, inSignal: questSignal);
    }

    private bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
    {
        return TileFinder.TryFindNewSiteTile(out tile, 10, 40, exitOnFirstTileFound: exitOnFirstTileFound);
    }

    protected override bool TestRunInt(Slate slate) => QuestGen_Get.GetMap() != null;
}
