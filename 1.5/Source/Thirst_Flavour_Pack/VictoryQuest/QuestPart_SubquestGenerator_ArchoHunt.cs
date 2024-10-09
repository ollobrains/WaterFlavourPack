using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Randomly spawn subquests at randomized intervals until we get the finished signal
/// </summary>
public class QuestPart_SubquestGenerator_ArchoHunt : QuestPart_SubquestGenerator
{
    private List<QuestScriptDef> questQueue = new List<QuestScriptDef>();
    public ThingDef archotechComponentDef;
    public string archotechComponentSlateName;
    public MapParent useMapParentThreatPoints;

    public override IEnumerable<GlobalTargetInfo> QuestLookTargets
    {
        get => Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentsSeenByPlayer.Where(t=>t.ParentHolder is not Building_ArchoMachine).Select(t=>(GlobalTargetInfo)t).Take(3);
    }
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

    protected override Slate InitSlate()
    {
        float var = 0.0f;
        if (useMapParentThreatPoints != null)
            var = !useMapParentThreatPoints.HasMap ? (Find.AnyPlayerHomeMap == null ? StorytellerUtility.DefaultThreatPointsNow(Find.World) : StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap)) : StorytellerUtility.DefaultThreatPointsNow(useMapParentThreatPoints.Map);
        Slate slate = new Slate();
        slate.Set("points", var);
        slate.Set(archotechComponentSlateName, archotechComponentDef);
        return slate;
    }

    protected override QuestScriptDef GetNextSubquestDef()
    {
        ShuffleQueue();
        QuestScriptDef nextSubquestDef = questQueue.First();
        if (!nextSubquestDef.CanRun(InitSlate()))
            return null;
        questQueue.RemoveAt(0);
        return nextSubquestDef;
    }

    private void ShuffleQueue()
    {
        questQueue.Clear();
        if (subquestDefs.Count == 1)
        {
            questQueue.AddRange(subquestDefs);
        }
        else
        {
            QuestScriptDef root = quest.GetSubquests().OrderByDescending(q => q.appearanceTick).FirstOrDefault()?.root;
            int num = 100;
            while (num > 0)
            {
                --num;
                questQueue.Clear();
                questQueue.AddRange(subquestDefs.InRandomOrder());
                if (root == null || questQueue.First() != root)
                    break;
            }
        }
    }


    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref archotechComponentDef, "archotechComponentDef");
        Scribe_Values.Look(ref archotechComponentSlateName, "archotechComponentSlateName");
        Scribe_Collections.Look(ref questQueue, "questQueue", LookMode.Def);
        Scribe_References.Look(ref useMapParentThreatPoints, "useMapParentThreatPoints");
        if (Scribe.mode != LoadSaveMode.PostLoadInit || questQueue != null)
            return;
        questQueue = new List<QuestScriptDef>();
    }

}
