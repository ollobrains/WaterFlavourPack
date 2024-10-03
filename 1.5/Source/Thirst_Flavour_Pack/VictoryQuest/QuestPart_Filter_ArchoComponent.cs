using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Filter_ArchoComponent : QuestPart_Filter
{
    protected override bool Pass(SignalArgs args)
    {
        return Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentSeenByPlayer;
    }
}
