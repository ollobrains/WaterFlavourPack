using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_WaterVictory: QuestNode
{
    protected override void RunInt()
    {
        if (!ModLister.CheckIdeology("Archonexus victory"))
            return;
        Quest quest = QuestGen.quest;
        Slate slate = QuestGen.slate;

        QuestPart_SubquestGenerator_WaterVictory waterVictory = new QuestPart_SubquestGenerator_WaterVictory();
        waterVictory.inSignalEnable = slate.Get<string>("inSignal");
        waterVictory.interval = new IntRange(0, 0);
        waterVictory.maxSuccessfulSubquests = 3;
        waterVictory.maxActiveSubquests = 1;
        waterVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory_FirstCycle);
        waterVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory_SecondCycle);
        waterVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory_ThirdCycle);

        quest.AddPart(waterVictory);
    }

    protected override bool TestRunInt(Slate slate) => true;
}
