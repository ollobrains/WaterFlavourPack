using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class GenStep_ArchospringResearchBuildings_ThirdCycle: GenStep_ArchonexusResearchBuildings_ThirdCycle
{
    protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
    {
        BaseGen.Generate();
    }
}
