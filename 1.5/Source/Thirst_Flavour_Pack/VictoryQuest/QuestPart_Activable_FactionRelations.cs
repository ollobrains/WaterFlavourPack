using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Activable_FactionRelations(Faction targetFaction, Faction questGiverFaction, FactionRelationKind kind): QuestPartActivable
{
    public Faction TargetFaction = targetFaction;
    public Faction QuestGiverFaction = questGiverFaction;
    public FactionRelationKind RelationKind = kind;

    public override IEnumerable<Faction> InvolvedFactions => [TargetFaction, QuestGiverFaction];

    public int NextCheck = 300;

    public override void QuestPartTick()
    {
        if(Find.TickManager.TicksGame < NextCheck)
            return;

        NextCheck = Find.TickManager.TicksGame + 300;

        switch (RelationKind)
        {
            case FactionRelationKind.Neutral when TargetFaction.GoodwillWith(Faction.OfPlayer) >= 0:
            case FactionRelationKind.Ally when TargetFaction.GoodwillWith(Faction.OfPlayer) >= 75:
                Complete();
                break;
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref TargetFaction, "TargetFaction");
        Scribe_References.Look(ref QuestGiverFaction, "QuestGiverFaction");
        Scribe_Values.Look(ref RelationKind, "RelationKind");
        Scribe_Values.Look(ref NextCheck, "NextCheck");
    }


    public override string DescriptionPart
    {
        get
        {
            int currentGoodwill = TargetFaction.GoodwillWith(Faction.OfPlayer);
            int targetGoodwill = RelationKind == FactionRelationKind.Ally ? 75 : 0;
            int goodwillNeeded = targetGoodwill - currentGoodwill;

            return "MSS_Thirst_FactionRelations_DescriptionPart".Translate(currentGoodwill, TargetFaction.def.LabelCap, TargetFaction.PlayerRelationKind.ToString(), goodwillNeeded, RelationKind.ToString());
        }
    }
}
