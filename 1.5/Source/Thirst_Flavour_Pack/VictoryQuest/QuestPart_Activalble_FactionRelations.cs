using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Activalble_FactionRelations(Faction targetFaction, Faction questGiverFaction, FactionRelationKind kind): QuestPartActivable
{
    public Faction TargetFaction = targetFaction;
    public Faction QuestGiverFaction = questGiverFaction;
    public FactionRelationKind RelationKind = kind;

    public override IEnumerable<Faction> InvolvedFactions => [TargetFaction, QuestGiverFaction];

    public int NextCheck = 60;

    public override void QuestPartTick()
    {
        if(Find.TickManager.TicksGame < NextCheck)
            return;

        NextCheck += 60;

        if(TargetFaction.PlayerRelationKind == RelationKind)
            Complete();
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
