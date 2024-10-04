using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory: QuestNode
{
    protected override void RunInt()
    {
        if (!ModLister.CheckIdeology("Archonexus victory"))
            return;
        Quest quest = QuestGen.quest;
        Slate slate = QuestGen.slate;

        string playerFoundArchoComponent = QuestGen.GenerateNewSignal("PlayerFoundArchoComponent");
        string outerNodeCompleted = QuestGen.GenerateNewSignal("OuterNodeCompleted");

        QuestPart_PassOutInterval part2 = new QuestPart_PassOutInterval(); // Keep retrying until we move on.
        part2.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted; // The hidden outer quest is automatically accepted
        part2.inSignalEnable = quest.AddedSignal; //playerFoundArchoComponent;
        part2.inSignalsDisable.Add(outerNodeCompleted);
        part2.ticksInterval = new IntRange(60, 60);
        part2.outSignals.Add(playerFoundArchoComponent);
        quest.AddPart(part2);

        QuestPart_Filter_ArchoComponent part3 = new QuestPart_Filter_ArchoComponent(); // Validate that the player has seen a component
        part3.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted; // The hidden outer quest is automatically accepted
        part3.inSignal = playerFoundArchoComponent;
        part3.outSignal = outerNodeCompleted;
        quest.AddPart(part3);

        //Start the main quest cycle
        QuestPart_SubquestGenerator_ArchospringVictory archospringVictory = new QuestPart_SubquestGenerator_ArchospringVictory();
        archospringVictory.inSignalEnable = part3.outSignal; //slate.Get<string>("inSignal");
        archospringVictory.interval = new IntRange(0, 0);
        archospringVictory.maxSuccessfulSubquests = 3;
        archospringVictory.maxActiveSubquests = 1;
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory_FirstCycle);
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory_SecondCycle);
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_EndGame_WaterVictory_ThirdCycle);

        quest.AddPart(archospringVictory);
    }

    protected override bool TestRunInt(Slate slate) => true;
}
