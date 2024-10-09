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
        Map map = QuestGen_Get.GetMap();

        slate.Set("map", map);

        // precreate our signals
        string timerFiredSignal = QuestGen.GenerateNewSignal("TimerFired");
        string filterPassedSignal = QuestGen.GenerateNewSignal("FilterPassed");

        // Acts as a timer, outputting it's signal based on the ticksInterval - allows us to keep retrying the filter
        QuestPart_PassOutInterval timer = new QuestPart_PassOutInterval(); // Keep retrying until we move on.
        timer.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted; // The hidden outer quest is automatically accepted
        timer.inSignalEnable = quest.AddedSignal; //enable immediately;
        timer.inSignalsDisable.Add(filterPassedSignal); // disable when the filter passes
        timer.ticksInterval = new IntRange(60, 600); // Interval to refire at
        timer.outSignals.Add(timerFiredSignal); // signal to fire on interval
        quest.AddPart(timer);

        QuestPart_Filter_ArchoComponent filter = new QuestPart_Filter_ArchoComponent(); // Validate that the player has seen a component
        filter.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted; // The hidden outer quest is automatically accepted
        filter.inSignal = timerFiredSignal; // Accept signal timer
        filter.outSignal = filterPassedSignal; // output signal to timer and victory quest
        quest.AddPart(filter);

        //Start the main quest cycle
        QuestPart_SubquestGenerator_ArchospringVictory archospringVictory = new QuestPart_SubquestGenerator_ArchospringVictory();
        archospringVictory.inSignalEnable = filterPassedSignal;
        archospringVictory.interval = Thirst_Flavour_PackMod.settings.ArchoQuestComponentHuntInterval; // interval between quests
        archospringVictory.maxSuccessfulSubquests = 4;
        archospringVictory.maxActiveSubquests = 1;
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_Thirst_EndGame_WaterVictory_PreCycle);
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_Thirst_EndGame_WaterVictory_FirstCycle);
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_Thirst_EndGame_WaterVictory_SecondCycle);
        archospringVictory.subquestDefs.Add(Thirst_Flavour_PackDefOf.MSS_Thirst_EndGame_WaterVictory_ThirdCycle);

        quest.AddPart(archospringVictory);
    }

    protected override bool TestRunInt(Slate slate) => true;
}
