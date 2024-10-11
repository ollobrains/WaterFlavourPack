using System.Collections.Generic;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class GenStep_ArchospringComponent: GenStep_ScattererBestFit
{
    public Thing GenerateBuildingComponent(ThingDef thingDef)
    {
        ThingDef stuff = Precept_Relic.GenerateStuffFor(thingDef);
        if (thingDef.MadeFromStuff)
        {
            stuff = Precept_Relic.GenerateStuffFor(thingDef);
        }
        Thing relic = thingDef.CompDefFor<CompQuality>() == null ? ThingMaker.MakeThing(thingDef) : new ThingStuffPairWithQuality(thingDef, stuff, QualityCategory.Legendary).MakeThing();
        return relic;
    }

    protected override bool TryFindScatterCell(Map map, out IntVec3 result)
    {
        if (!base.TryFindScatterCell(map, out result))
            result = map.Center;
        return true;
    }

    protected override IntVec2 Size => new IntVec2(3, 3);

    public override bool CollisionAt(IntVec3 cell, Map map)
    {
        TerrainDef terrain = cell.GetTerrain(map);
        if (terrain != null && (terrain.IsWater || terrain.IsRoad))
            return true;
        List<Thing> thingList = cell.GetThingList(map);
        foreach (Thing t in thingList)
        {
            if (t.def.IsBuildingArtificial || t.def.building != null && t.def.building.isNaturalRock)
                return true;
        }
        return false;
    }

    protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
    {
        ResolveParams resolveParams = new ResolveParams
        {
            rect = CellRect.CenteredOn(c, Size.x, Size.z), singleThingToSpawn = ((SitePartWorker_BarbCamp) parms.sitePart.def.Worker).component,
            sitePart = parms.sitePart
        };
        BaseGen.globalSettings.map = map;
        BaseGen.symbolStack.Push("mss_thirst_archo_component", resolveParams);
        BaseGen.Generate();
        parms.sitePart.relicWasSpawned = true;
    }

    public override int SeedPart => 245123523;
}
