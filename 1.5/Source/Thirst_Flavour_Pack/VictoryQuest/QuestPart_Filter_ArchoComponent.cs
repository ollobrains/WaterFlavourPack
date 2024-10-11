using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Filter to check if the player has seen an archo component ever.
/// Adds hyperlinks to them
/// </summary>
public class QuestPart_Filter_ArchoComponent : QuestPart_Filter
{
    protected override bool Pass(SignalArgs args)
    {
        return Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentSeenByPlayer;
    }
}
