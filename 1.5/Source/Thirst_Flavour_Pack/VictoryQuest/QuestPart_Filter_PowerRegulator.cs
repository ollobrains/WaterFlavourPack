using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Filter_PowerRegulator(int count = 3) : QuestPart_Filter
{
    public int Count = count;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Count, "Count");
    }

    protected override bool Pass(SignalArgs args)
    {
        return ArchospringVictoryWorldComponent.Instance.PowerRegulatorsBuilt >= Count;
    }

    public override IEnumerable<GlobalTargetInfo> QuestLookTargets
    {
        get => Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentsSeenByPlayer.Select(t=>(GlobalTargetInfo)t);
    }
}
