using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Filter_ArchoSpringBuilding( ThingDef def, int count = 3) : QuestPart_Filter
{
    public int Count = count;
    public ThingDef Def = def;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Count, "Count");
        Scribe_Defs.Look(ref Def, "Def");
    }

    public int Current => Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingComponentCount.GetWithFallback(Def, 0);

    protected override bool Pass(SignalArgs args)
    {
        return Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingComponentCount.GetWithFallback(Def, 0) >= Count;
    }

    public override IEnumerable<GlobalTargetInfo> QuestLookTargets
    {
        get => Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentsSeenByPlayer.Where(t=>t is { Destroyed: false }).Select(t=>(GlobalTargetInfo)t);
    }

    public override string DescriptionPart => "MSS_Thirst_QuestPart_Filter_PowerRegulator_DescPart".Translate(Current, Count, Def.LabelCap);
}
