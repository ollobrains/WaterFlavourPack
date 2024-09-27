using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class SymbolResolver_Archospring: SymbolResolver
{
    private const int SuperstructureDistance = 11;
    private const int MinorSuperstructureDistance = 28;
    private const int MinorSuperstructureCount = 9;
    private static List<CellRect> MinorSupersturctureSites = new List<CellRect>();
    private static List<CellRect> MajorSupersturctureSites = new List<CellRect>();
    private const string MechanoidsWakeUpSignalPrefix = "ArchonexusMechanoidsWakeUp";

    public TerrainDef BaseTerrainDef => TerrainDefOf.FlagstoneSandstone;
    public TerrainDef PathTerrainDef => TerrainDefOf.TileSandstone;

    public override void Resolve(ResolveParams rp)
    {
      rp.floorDef = BaseTerrainDef;
      rp.chanceToSkipFloor = 0.0f;
      MinorSupersturctureSites.Clear();
      MajorSupersturctureSites.Clear();
      // // destroy plants and buildings
      foreach (IntVec3 intVec3 in rp.rect)
      {
          if (intVec3.DistanceTo(rp.rect.CenterCell) <= 28.0)
          {
              Plant plant = intVec3.GetPlant(BaseGen.globalSettings.map);
              if (plant != null && plant.def.destroyable)
                  plant.Destroy(DestroyMode.Vanish);
              Building edifice = intVec3.GetEdifice(BaseGen.globalSettings.map);
              if (edifice != null && edifice.def.destroyable)
                  edifice.Destroy(DestroyMode.Vanish);
              BaseGen.globalSettings.map.roofGrid.SetRoof(intVec3, null);
          }
      }

      // // Add sleeping mechs
      // float? threatPoints = rp.threatPoints;
      // if (threatPoints.GetValueOrDefault() > (double) 0.0f & threatPoints.HasValue && Faction.OfMechanoids != null)
      // {
      //   string mechsWakeUpSignal = "ArchonexusMechanoidsWakeUp" + Find.UniqueIDsManager.GetNextSignalTagID();
      //   ResolveParams triggerParams = rp with
      //   {
      //     rect = rp.rect.ExpandedBy(5),
      //     rectTriggerSignalTag = mechsWakeUpSignal,
      //     threatPoints = rp.threatPoints
      //   };
      //   BaseGen.symbolStack.Push("rectTrigger", triggerParams);
      //   ResolveParams mechanoidSleepParams = rp with
      //   {
      //     sleepingMechanoidsWakeupSignalTag = mechsWakeUpSignal,
      //     threatPoints = rp.threatPoints
      //   };
      //   BaseGen.symbolStack.Push("sleepingMechanoids", mechanoidSleepParams);
      //   ResolveParams mechanoidWakeUpParams = rp with
      //   {
      //     sound = SoundDefOf.ArchonexusThreatsAwakened_Alarm,
      //     soundOneShotActionSignalTag = mechsWakeUpSignal
      //   };
      //   BaseGen.symbolStack.Push("soundOneShotAction", mechanoidWakeUpParams);
      // }

      Thing.allowDestroyNonDestroyable = true;
      foreach (IntVec3 intVec3 in GenRadial.RadialCellsAround(rp.rect.CenterCell, 50f, true))
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
      Thing.allowDestroyNonDestroyable = false;
      // Add corpses
      ResolveParams corpseParams = rp with
      {
        dessicatedCorpseDensityRange = new FloatRange(3f / 1000f, 3f / 500f)
      };
      BaseGen.symbolStack.Push("desiccatedCorpses", corpseParams);

      // Add the archonexus core
      ResolveParams archonexusCoreParams = rp with
      {
        rect = CellRect.CenteredOn(rp.rect.CenterCell, ThingDefOf.ArchonexusCore.size.x, ThingDefOf.ArchonexusCore.size.z),
        singleThingDef = ThingDefOf.ArchonexusCore,
        floorDef = BaseTerrainDef
      };
      BaseGen.symbolStack.Push("thing", archonexusCoreParams);
      archonexusCoreParams.rect = archonexusCoreParams.rect.ExpandedBy(1);
      BaseGen.symbolStack.Push("floor", archonexusCoreParams);
      BaseGen.symbolStack.Push("clear", archonexusCoreParams);

      // Add the 4 main sub-structures
      Vector3 direction = IntVec3.North.ToVector3();

      for (int index = 0; index < 4; ++index)
      {
        IntVec3 diagonalDirection = GenAdj.DiagonalDirections[index];
        CellRect cellRect = CellRect.CenteredOn(rp.rect.CenterCell + diagonalDirection * 11, ThingDefOf.MajorArchotechStructure.size.x, ThingDefOf.MajorArchotechStructure.size.z);
        ResolveParams archoStructParams = rp;
        Thing thing = ThingMaker.MakeThing(ThingDefOf.MajorArchotechStructure);
        thing.TryGetComp<CompSpawnImmortalSubplantsAround>()?.Disable();
        archoStructParams.singleThingToSpawn = thing;
        archoStructParams.rect = cellRect;
        BaseGen.symbolStack.Push("thing", archoStructParams);
        ResolveParams floorParams = archoStructParams with
        {
          rect = archoStructParams.rect,
          clearRoof = true,
          floorDef = BaseTerrainDef
        };
        BaseGen.symbolStack.Push("floor", floorParams);
        BaseGen.symbolStack.Push("clear", floorParams);
        MajorSupersturctureSites.Add(cellRect);
      }

      // Add the 9 minor sub structures
      float angleOffset = 40f;
      for (int index = 0; index < 9; ++index)
      {
        float angle = index * angleOffset;
        Vector3 vect = direction.RotatedBy(angle) * 28f;
        CellRect cellRect = CellRect.CenteredOn(rp.rect.CenterCell + vect.ToIntVec3(), ThingDefOf.ArchotechTower.size.x, ThingDefOf.ArchotechTower.size.z);
        ResolveParams minorArchoStructParams = rp with
        {
          singleThingDef = ThingDefOf.ArchotechTower,
          rect = cellRect,
          floorDef = BaseTerrainDef
        };
        BaseGen.symbolStack.Push("thing", minorArchoStructParams);
        ResolveParams minorArchoStructFloorParams = minorArchoStructParams with
        {
          rect = minorArchoStructParams.rect,
          clearRoof = true
        };
        BaseGen.symbolStack.Push("floor", minorArchoStructFloorParams);
        BaseGen.symbolStack.Push("clear", minorArchoStructFloorParams);
        MinorSupersturctureSites.Add(cellRect);
      }
      rp.chanceToSkipFloor = 0.95f;
      BaseGen.symbolStack.Push("floor", rp);

      List<List<IntVec3>> occupiedCells = new List<List<IntVec3>>();

      // Water
      List<IntVec3> coreWaterCells = GenRadial.RadialCellsAround(rp.rect.CenterCell, 25f, true).ToList();
      occupiedCells.Add(coreWaterCells);
      foreach (IntVec3 cell in coreWaterCells)
      {
          BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, TerrainDefOf.WaterShallow);
      }

      foreach (CellRect majorSupersturctureSite in MajorSupersturctureSites)
      {
          List<IntVec3> cells = GenRadial.RadialCellsAround(majorSupersturctureSite.CenterCell, 20f, true).ToList();
          occupiedCells.Add(cells);
          foreach (IntVec3 cell in cells)
          {
              BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, TerrainDefOf.WaterShallow);
          }
      }
      foreach (CellRect minorSupersturctureSite in MinorSupersturctureSites)
      {
          List<IntVec3> cells = GenRadial.RadialCellsAround(minorSupersturctureSite.CenterCell, 20f, true).ToList();
          occupiedCells.Add(cells);
          foreach (IntVec3 cell in cells)
          {
              BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, TerrainDefOf.WaterShallow);
          }
      }

      // floor
      foreach (IntVec3 cell in GenRadial.RadialCellsAround(rp.rect.CenterCell, 8f, true))
      {
          BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, BaseTerrainDef);
      }

      foreach (CellRect majorSupersturctureSite in MajorSupersturctureSites)
      {
          foreach (IntVec3 cell in GenRadial.RadialCellsAround(majorSupersturctureSite.CenterCell, 6f, true))
          {
              BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, BaseTerrainDef);
          }
      }
      foreach (CellRect minorSupersturctureSite in MinorSupersturctureSites)
      {
          foreach (IntVec3 cell in GenRadial.RadialCellsAround(minorSupersturctureSite.CenterCell, 4f, true))
          {
              BaseGen.globalSettings.map.terrainGrid.SetTerrain(cell, BaseTerrainDef);
          }
      }

      // Ensure pathways
      for (int index = 0; index < MajorSupersturctureSites.Count; ++index)
        BaseGenUtility.DoPathwayBetween(archonexusCoreParams.rect.CenterCell, MajorSupersturctureSites[index].CenterCell, PathTerrainDef, 2);

      for (int index1 = 0; index1 < MinorSupersturctureSites.Count; ++index1)
      {
        CellRect current = MinorSupersturctureSites[index1];
        int index2 = GenMath.PositiveMod(index1 - 1, MinorSupersturctureSites.Count);
        BaseGenUtility.DoPathwayBetween(MinorSupersturctureSites[index2].CenterCell, current.CenterCell, rp.floorDef, 2);
        BaseGenUtility.DoPathwayBetween(MajorSupersturctureSites.MinBy(c => c.CenterCell.DistanceToSquared(current.CenterCell)).CenterCell, current.CenterCell, PathTerrainDef, 2);
      }

      // more pathways
      BaseGenUtility.DoPathwayBetween(archonexusCoreParams.rect.CenterCell, archonexusCoreParams.rect.CenterCell + IntVec3.South * 25, PathTerrainDef);


      IEnumerable<IntVec3> outerRadial = GenRadial.RadialCellsAround(rp.rect.CenterCell, 50f, true).Where(cell=>!occupiedCells.SelectMany(i=>i).Contains(cell));

      foreach (IntVec3 cell in outerRadial)
      {
          List<Thing> things = cell.GetThingList(BaseGen.globalSettings.map);
          foreach (Thing thing in things)
          {
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

      foreach (IntVec3 cell in GenRadial.RadialCellsAround(rp.rect.CenterCell, 25f, true))
      {
          if (cell.GetFirstBuilding(BaseGen.globalSettings.map) == null)
          {
              BaseGen.globalSettings.map.terrainGrid.SetUnderTerrain(rp.rect.CenterCell, TerrainDefOf.WaterShallow);
              BaseGen.globalSettings.map.terrainGrid.SetTerrain(rp.rect.CenterCell, TerrainDefOf.WaterShallow);
          }
      }

      List<CellRect> var;
      if (!MapGenerator.TryGetVar("UsedRects", out var))
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
