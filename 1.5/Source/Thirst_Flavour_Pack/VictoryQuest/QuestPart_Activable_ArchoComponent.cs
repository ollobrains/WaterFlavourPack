using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_Activable_ArchoComponent : QuestPartActivable
{
    public override void QuestPartTick()
    {
        if (Find.TickManager.TicksGame % 60 != 0 || !Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentSeenByPlayer)
            return;
        Complete();
    }
}
