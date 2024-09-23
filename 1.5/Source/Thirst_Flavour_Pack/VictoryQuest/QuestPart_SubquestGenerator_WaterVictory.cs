using System.Linq;
using RimWorld;
using RimWorld.QuestGen;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_SubquestGenerator_WaterVictory : QuestPart_SubquestGenerator
{
    protected override Slate InitSlate()
    {
        Slate slate = new Slate();
        return slate;
    }

    protected override QuestScriptDef GetNextSubquestDef()
    {
        QuestScriptDef subquestDef = subquestDefs[quest.GetSubquests(QuestState.EndedSuccess).Count() % subquestDefs.Count];
        return !subquestDef.CanRun(InitSlate()) ? null : subquestDef;
    }
}
