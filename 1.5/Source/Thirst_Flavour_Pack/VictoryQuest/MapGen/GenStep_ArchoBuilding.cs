using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class GenStep_ArchoBuilding : GenStep_Archonexus
{
    public ThingDef BuildingDef;

    protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
    {
        SitePartParams parms1 = parms.sitePart.parms;
        ResolveParams resolveParams = new ResolveParams { threatPoints = parms1.threatPoints, rect = CellRect.CenteredOn(c, 60, 60) };
        if(BuildingDef != null)
            resolveParams.singleThingDef = BuildingDef;
        BaseGen.globalSettings.map = map;
        BaseGen.symbolStack.Push("mssthirstarchobuilding", resolveParams);
        BaseGen.Generate();
    }
}
