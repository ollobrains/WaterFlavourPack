using System.Linq;
using RimWorld;
using RimWorld.QuestGen;
using Thirst_Flavour_Pack.VictoryQuest.PreQuest;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_SubquestGenerator_WaterVictory : QuestPart_SubquestGenerator//, ISignalReceiver
{
    // public bool SigReceived = false;
    // public void Notify_SignalReceived(Signal signal)
    // {
    //     if (signal.tag == QuestNode_Root_WaterVictory_PreCycle.QuestSignal)
    //     {
    //         SigReceived = true;;
    //     }
    // }

    public override void ExposeData()
    {
        base.ExposeData();
        // Scribe_Values.Look(ref SigReceived, "SigReceived");
    }
    protected override Slate InitSlate()
    {
        Slate slate = new Slate();
        return slate;
    }

    protected override QuestScriptDef GetNextSubquestDef()
    {
        // if (!SigReceived) return null;
        QuestScriptDef subquestDef = subquestDefs[quest.GetSubquests(QuestState.EndedSuccess).Count() % subquestDefs.Count];
        return !subquestDef.CanRun(InitSlate()) ? null : subquestDef;
    }
}
