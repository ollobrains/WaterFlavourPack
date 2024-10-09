using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_BecomeNeutral: QuestNode
{
    protected override void RunInt()
    {
        Quest quest = QuestGen.quest;
        Slate slate = QuestGen.slate;
        Map map = QuestGen_Get.GetMap();

        slate.Set("map", map);

        string relationReached = QuestGen.GenerateNewSignal("MSS_Thirst_Relation_Reached");

        Pawn asker = FindAsker();
        slate.Set("asker", asker);

        Faction targetFaction = Find.FactionManager.GetFactions(allowNonHumanlike: false, allowTemporary: false).Where(f=>f.def.permanentEnemy == false && f.HostileTo(Find.FactionManager.OfPlayer)).RandomElement();
        slate.Set("targetFaction", targetFaction);
        FactionRelationKind targetRelationKind = Rand.Bool ? FactionRelationKind.Neutral : FactionRelationKind.Ally;
        slate.Set("targetRelationKind", targetRelationKind);

        QuestPart_Activable_FactionRelations questCompleteActivable = new QuestPart_Activable_FactionRelations(targetFaction, asker.Faction, targetRelationKind);
        questCompleteActivable.inSignalEnable = quest.InitiateSignal;
        questCompleteActivable.outSignalsCompleted.Add(relationReached);

        quest.AddPart(questCompleteActivable);

        quest.End(QuestEndOutcome.Success, inSignal:relationReached, sendStandardLetter: true);

        List<Thing> rewards = [ThingMaker.MakeThing(Thirst_Flavour_PackDefOf.MSS_Thirst_ComponentArcho)];


        Reward_Items reward = new Reward_Items();
        reward.items = rewards;

        QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
        choice.rewards.Add(reward);

        RewardsGeneratorParams rgp = new RewardsGeneratorParams();
        rgp.giveToCaravan = false;

        foreach (QuestPart questPart in reward.GenerateQuestParts(0, rgp, null, null, null, null))
        {
            QuestGen.quest.AddPart(questPart);
            choice.questParts.Add(questPart);
        }
    }

    protected override bool TestRunInt(Slate slate) => true;

    private Pawn FindAsker()
    {
        return Find.FactionManager.AllFactionsVisible.Where(f => f.def.humanlikeFaction && !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && f.def.techLevel > TechLevel.Neolithic && f.leader != null && !f.temporary && !f.Hidden).TryRandomElement(out Faction result) ? result.leader : null;
    }
}
