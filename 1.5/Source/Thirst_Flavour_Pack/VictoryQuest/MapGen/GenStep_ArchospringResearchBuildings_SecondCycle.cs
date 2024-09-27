using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class GenStep_ArchospringResearchBuildings_SecondCycle: GenStep_ArchonexusResearchBuildings_SecondCycle
{
    protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
    {
        RimWorld.BaseGen.BaseGen.Generate();
    }
}
