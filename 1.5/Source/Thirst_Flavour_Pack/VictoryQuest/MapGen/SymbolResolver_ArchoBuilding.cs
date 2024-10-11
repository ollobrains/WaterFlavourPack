using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class SymbolResolver_ArchoBuilding : SymbolResolver
{
    private const int SuperstructureDistance = 11;
    private const int MinorSuperstructureDistance = 28;
    private const int MinorSuperstructureCount = 9;
    private static List<CellRect> MinorSupersturctureSites = [];
    private static List<CellRect> MajorSupersturctureSites = [];
    private const string MechanoidsWakeUpSignalPrefix = "ArchonexusMechanoidsWakeUp";

    public TerrainDef BaseTerrainDef => TerrainDefOf.FlagstoneSandstone;
    public TerrainDef PathTerrainDef => TerrainDefOf.TileSandstone;

    public override void Resolve(ResolveParams rp)
    {
        rp.floorDef = BaseTerrainDef;
        rp.chanceToSkipFloor = 0.0f;
        MinorSupersturctureSites.Clear();
        MajorSupersturctureSites.Clear();

        Thing.allowDestroyNonDestroyable = true;
        try
        {
            foreach (IntVec3 intVec3 in GenRadial.RadialCellsAround(rp.rect.CenterCell, 10f, true))
            {
                BaseGen.globalSettings.map.roofGrid.SetRoof(intVec3, null);

                List<Thing> thingsAt = intVec3.GetThingList(BaseGen.globalSettings.map);
                List<Thing> thingsToDestroy = new List<Thing>();
                thingsToDestroy.AddRange(thingsAt);

                foreach (Thing thing in thingsToDestroy)
                {
                    try
                    {
                        thing.Destroy(DestroyMode.Vanish);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Exception thrown while trying to destroy thing: {e}. Continuing");
                    }
                }
            }
        }
        finally
        {
            Thing.allowDestroyNonDestroyable = false;
        }

        // Add corpses
        ResolveParams corpseParams = rp with { dessicatedCorpseDensityRange = new FloatRange(3f / 1000f, 3f / 500f) };
        BaseGen.symbolStack.Push("desiccatedCorpses", corpseParams);

        // Add the archonexus core
        ResolveParams powerRegulatorParams = rp with
        {
            faction = Faction.OfPlayer,
            rect = CellRect.CenteredOn(rp.rect.CenterCell, rp.singleThingDef.size.x, rp.singleThingDef.size.z),
            singleThingDef = rp.singleThingDef,
            floorDef = BaseTerrainDef
        };
        BaseGen.symbolStack.Push("thing", powerRegulatorParams);
        powerRegulatorParams.rect = powerRegulatorParams.rect.ExpandedBy(1);

        BaseGen.symbolStack.Push("floor", powerRegulatorParams);
        BaseGen.symbolStack.Push("clear", powerRegulatorParams);

        rp.chanceToSkipFloor = 0.95f;
        BaseGen.symbolStack.Push("floor", rp);

        List<List<IntVec3>> occupiedCells = new List<List<IntVec3>>();

        // Water
        List<IntVec3> coreWaterCells = GenRadial.RadialCellsAround(rp.rect.CenterCell, 10f, true).ToList();
        occupiedCells.Add(coreWaterCells);
        foreach (IntVec3 cell in coreWaterCells)
        {
            BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, TerrainDefOf.WaterShallow);
        }

        // floor
        foreach (IntVec3 cell in GenRadial.RadialCellsAround(rp.rect.CenterCell, 6f, true))
        {
            BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, BaseTerrainDef);
        }

        // Ensure pathways
        BaseGenUtility.DoPathwayBetween(powerRegulatorParams.rect.CenterCell, powerRegulatorParams.rect.CenterCell + IntVec3.South * 10, PathTerrainDef);


        IEnumerable<IntVec3> outerRadial = GenRadial.RadialCellsAround(rp.rect.CenterCell, 14f, true).Where(cell => !occupiedCells.SelectMany(i => i).Contains(cell));

        foreach (IntVec3 cell in outerRadial)
        {
            List<Thing> things = cell.GetThingList(BaseGen.globalSettings.map);
            for (int i = 0; i < things.Count; i++)
            {
                Thing thing = things[i];
                if (thing.def.category == ThingCategory.Plant && thing.def != ThingDefOf.Plant_TreeAnima)
                    thing.Destroy();
            }

            if (cell.GetTerrain(BaseGen.globalSettings.map).passability != Traversability.Impassable)
            {
                Plant grass = (Plant) GenSpawn.Spawn(ThingDefOf.Plant_GrassAnima, cell, BaseGen.globalSettings.map);
                grass.Growth = 1f;
                BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, TerrainDefOf.SoilRich);
            }
        }

        if (!MapGenerator.TryGetVar("UsedRects", out List<CellRect> var))
        {
            var = [];
            MapGenerator.SetVar("UsedRects", var);
        }

        var.AddRange(MinorSupersturctureSites);
        var.AddRange(MajorSupersturctureSites);

        MinorSupersturctureSites.Clear();
        MajorSupersturctureSites.Clear();
    }
}
