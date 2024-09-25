using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class SymbolResolver_Archospring: SymbolResolver_Archonexus
{
    public override void Resolve(ResolveParams rp)
    {
        base.Resolve(rp);

        foreach (IntVec3 cell in GenRadial.RadialCellsAround(rp.rect.CenterCell, 15f, false))
        {
            if (cell.GetFirstBuilding(BaseGen.globalSettings.map) == null)
            {
                BaseGen.globalSettings.map.terrainGrid.SetUnderTerrain(rp.rect.CenterCell, TerrainDefOf.WaterShallow);
                BaseGen.globalSettings.map.terrainGrid.SetTerrain(rp.rect.CenterCell, TerrainDefOf.WaterShallow);
            }
        }
    }

}
