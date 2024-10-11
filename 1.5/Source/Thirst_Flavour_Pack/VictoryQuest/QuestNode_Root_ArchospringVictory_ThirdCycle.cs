using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Thirst_Flavour_Pack.VictoryQuest.MapGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_ThirdCycle : QuestNode_Root_ArchospringVictory_Cycle
{
    protected override int QuestCycle => 3;

    protected override SitePartDef CurrentSitePartDef => Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_SterilizationPlant_Site;
    protected override QuestPartActivable_BuildingUnavailable BuildingFilter => new QuestPartActivable_BuildingUnavailable(Thirst_Flavour_PackDefOf.MSS_Thirst_SterilizationPlant);

    protected override ThingDef BuildingDef => Thirst_Flavour_PackDefOf.MSS_Thirst_SterilizationPlant;

    private static float ThreatPointsFactor = 0.6f;
    protected override bool SpawnSite => true;
    protected override bool SetSuccess => false;

    private static FloatRange RandomRaidPointsFactorRange => Thirst_Flavour_PackMod.settings.FinalFightRaidFactor;

    public static string ArchospringActivatingSignal = "MSS_Thirst_ArchospringActivating";
    public static string AllEnemiesDefeatedOutSignal => DefeatAllEnemiesArchospringQuestComp.AllEnemiesDefeated_Archospring_Signal;

    public Lazy<FieldInfo> QuestPart_PassOutInterval_currentInterval = new Lazy<FieldInfo>(() => AccessTools.Field(typeof(QuestPart_PassOutInterval), "currentInterval"));

    protected override void RunInt()
    {
        ModLog.Debug("QuestNode_Root_ArchospringVictory_ThirdCycle.RunInt");
        base.RunInt();
        Quest quest = QuestGen.quest;
        Slate slate = QuestGen.slate;

        Faction enemyFaction = Thirst_Flavour_PackMod.settings.FinalFightFaction;

        if (enemyFaction == null || enemyFaction.IsPlayer || enemyFaction.PlayerGoodwill >= 0)
        {
            enemyFaction = Find.FactionManager.RandomEnemyFaction();
        }

        // Fire up the dialog and a letter
        quest.DialogWithCloseBehavior(
            "[questDescriptionBeforeAccepted]",
            inSignal: quest.AddedSignal,
            signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly,
            closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd);

        quest.Letter(LetterDefOf.PositiveEvent, text: "[questAcceptedLetterText]", label: "[questAcceptedLetterLabel]");


        // grab a free tile for the site
        TryFindSiteTile(out int tile);

        float raidPoints = slate.Get<float>("points");
        float adjustedRaidPoints = Find.Storyteller.difficulty.allowViolentQuests ? raidPoints * ThreatPointsFactor : 0.0f;
        SitePartParams parms = new SitePartParams { threatPoints = adjustedRaidPoints };
        Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(Thirst_Flavour_PackDefOf.MSS_Thirst_ArchospringSite, parms)), tile, Faction.OfAncients);
        if (raidPoints <= 0.0 && Find.Storyteller.difficulty.allowViolentQuests)
            quest.SetSitePartThreatPointsToCurrent(site, Thirst_Flavour_PackDefOf.MSS_Thirst_ArchospringSite, map.Parent, threatPointsFactor: ThreatPointsFactor);
        quest.SpawnWorldObject(site);


        if (Find.Storyteller.difficulty.allowViolentQuests && Thirst_Flavour_PackMod.settings.EnableFinalQuestFight)
        {
            string doRaidSignal = QuestGen.GenerateNewSignal("MSS_Thirst_DoRaid");

            // Pop up a dialog to explain that a raid is incoming
            quest.Dialog("MSS_Thirst_ActivateArchospringText".Translate(), inSignal: ArchospringActivatingSignal);

            // Use this as a delay to pause the raid until 5-10 minutes have passed
            QuestPart_PassOutInterval delay = new QuestPart_PassOutInterval();
            delay.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
            delay.inSignalEnable = ArchospringActivatingSignal;
            delay.inSignalDisable = doRaidSignal;
            delay.ticksInterval = new IntRange(18000, 36000);
            delay.outSignals.Add(doRaidSignal);

            // force set the interval so that it doesn't fire immediately
            QuestPart_PassOutInterval_currentInterval.Value.SetValue(delay, delay.ticksInterval.RandomInRange);
            quest.AddPart(delay);

            // Once the raid has started, tell the comp to start monitoring for all enemies defeated
            DefeatAllEnemiesArchospringQuestComp comp = site.GetComponent<DefeatAllEnemiesArchospringQuestComp>();
            QuestPart_ActionOnSignal action = new QuestPart_ActionOnSignal();
            action.inSignal = doRaidSignal;
            action.inSignalEnable = ArchospringActivatingSignal;
            action.action = () =>
            {
                comp.StartQuest();
            };

            quest.AddPart(action);

            // actually trigger the raid
            quest.RandomRaid(site, RandomRaidPointsFactorRange * raidPoints, enemyFaction, doRaidSignal,
                PawnsArrivalModeDefOf.EdgeWalkIn,
                RaidStrategyDefOf.ImmediateAttack);

            // trigger the ending
            QuestPart_Ending ending = new QuestPart_Ending();
            ending.inSignal = AllEnemiesDefeatedOutSignal;

            quest.AddPart(ending);
        }
        else
        {
            // trigger the ending
            QuestPart_Ending ending = new QuestPart_Ending();
            ending.inSignal = ArchospringActivatingSignal;

            quest.AddPart(ending);
        }


        slate.Set("factionless", true);
        slate.Set("threatsEnabled", Find.Storyteller.difficulty.allowViolentQuests);
    }

    protected override bool TestRunInt(Slate slate)
    {
        return base.TestRunInt(slate) && TryFindSiteTile(out int _, true);
    }
}
