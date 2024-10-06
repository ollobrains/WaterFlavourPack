using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Filter_ArchoComponent : QuestPart_Filter
{
    public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
    {
        get
        {
            foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
                yield return hyperlink;
            foreach (Thing outerThing in QuestLookTargets)
                yield return new Dialog_InfoCard.Hyperlink(outerThing.GetInnerIfMinified().def);
        }
    }

    public override IEnumerable<GlobalTargetInfo> QuestLookTargets
    {
        get => Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentsSeenByPlayer.Select(t=>(GlobalTargetInfo)t).Take(3);
    }

    protected override bool Pass(SignalArgs args)
    {
        return Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentSeenByPlayer;
    }
}
