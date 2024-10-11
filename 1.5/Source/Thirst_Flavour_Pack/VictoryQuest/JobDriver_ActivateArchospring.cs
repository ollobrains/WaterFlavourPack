using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class JobDriver_ActivateArchospring: JobDriver_ActivateArchonexusCore
{
    protected override IEnumerable<Toil> MakeNewToils()
    {
        if (!ModLister.CheckIdeology("Activate archonexus core"))
        {
            yield break;
        }
        yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell);
        Toil f = ToilMaker.MakeToil("MakeNewToils");
        f.initAction = delegate
        {
            SignalArgs args = new SignalArgs(new NamedArgument(job.targetA.Thing, "MSS_Thirst_Archospring"));
            Find.SignalManager.SendSignal(new Signal(QuestNode_Root_ArchospringVictory_ThirdCycle.ArchospringActivatingSignal, args, true));
        };
        f.handlingFacing = true;
        f.tickAction = delegate
        {
            pawn.rotationTracker.FaceTarget(TargetA);
        };
        f.defaultCompleteMode = ToilCompleteMode.Delay;
        f.defaultDuration = 120;
        f.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
        yield return f;
    }
}
