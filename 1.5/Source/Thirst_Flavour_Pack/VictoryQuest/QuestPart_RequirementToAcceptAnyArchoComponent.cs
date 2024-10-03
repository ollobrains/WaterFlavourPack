using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptAnyArchoComponent: QuestPart_RequirementsToAccept
{
    public override AcceptanceReport CanAccept()
    {
        return Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentSeenByPlayer;
    }
}
