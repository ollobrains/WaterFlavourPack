using System;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Run arbitrary action on signal
/// </summary>
public class QuestPart_ActionOnSignal: QuestPartActivable
{
    public string inSignal;
    public Action action;

    public void DoEnable()
    {
        Enable(new SignalArgs());
    }
    protected override void ProcessQuestSignal(Signal signal)
    {
        base.ProcessQuestSignal(signal);
        if (signal.tag != inSignal)
            return;
        ModLog.Debug($"Signal \"{signal}\" called, triggering action \"{action}\" ");
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
        inSignal = $"DebugSignal{Rand.Int}";
    }
}
