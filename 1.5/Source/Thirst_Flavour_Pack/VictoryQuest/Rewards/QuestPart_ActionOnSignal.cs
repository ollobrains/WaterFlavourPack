using System;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.Rewards;

public class QuestPart_ActionOnSignal: QuestPartActivable
{
    public string inSignal;
    public Action action;

    protected override void ProcessQuestSignal(Signal signal)
    {
        base.ProcessQuestSignal(signal);
        if (signal.tag != inSignal)
            return;
        action?.Invoke();
        Complete(signal.args);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref inSignal, "inSignal");
        Scribe_Values.Look(ref action, "action");
    }

    public override void AssignDebugData()
    {
        base.AssignDebugData();
        this.inSignal = "DebugSignal" + (object) Rand.Int;
    }
}
