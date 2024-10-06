using System.Linq;
using RimWorld;
using RimWorld.QuestGen;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Spawns the next subquest in the sequence
/// </summary>
public class QuestPart_SubquestGenerator_ArchospringVictory : QuestPart_SubquestGenerator
{
    protected override Slate InitSlate()
    {
        Slate slate = new Slate();
        return slate;
    }

    protected override QuestScriptDef GetNextSubquestDef()
    {
        // if (!SigReceived) return null;
        QuestScriptDef subquestDef = subquestDefs[quest.GetSubquests(QuestState.EndedSuccess).Count() % subquestDefs.Count];
        return !subquestDef.CanRun(InitSlate()) ? null : subquestDef;
    }
}
