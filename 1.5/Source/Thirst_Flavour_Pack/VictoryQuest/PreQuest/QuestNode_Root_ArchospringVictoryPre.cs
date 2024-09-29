using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.PreQuest;

public class QuestNode_Root_ArchospringVictoryPre: QuestNode
{
    protected override void RunInt()
    {
        if (!ModLister.CheckIdeology("Archonexus victory"))
            return;
        Quest quest = QuestGen.quest;
        Slate slate = QuestGen.slate;

        QuestPart_SubquestGenerator_ArchospringVictory archospringVictory = new QuestPart_SubquestGenerator_ArchospringVictory();
        archospringVictory.inSignalEnable = slate.Get<string>("inSignal");
        archospringVictory.interval = new IntRange(0, 0);
        archospringVictory.maxSuccessfulSubquests = 2;
        archospringVictory.maxActiveSubquests = 1;
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory_PreCycle);
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory);

        quest.AddPart(archospringVictory);
    }

    protected override bool TestRunInt(Slate slate) => true;
}
