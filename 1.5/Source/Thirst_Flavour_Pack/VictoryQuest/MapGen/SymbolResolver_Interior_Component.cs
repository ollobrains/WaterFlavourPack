using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class SymbolResolver_Interior_Component : SymbolResolver
{
    public override void Resolve(ResolveParams rp)
    {
        IntVec3 max = rp.rect.Max;
        max.x -= rp.rect.Width / 2;
        --max.z;
        CellRect rect = rp.rect;
        rect.maxZ -= 3;

        if (rp.sitePart == null)
            return;
        rp.singleThingToSpawn = rp.sitePart.things.FirstOrDefault();

        rp.rect = CellRect.CenteredOn(max, 1, 1);
        rp.thingRot = Rot4.South;
        BaseGen.symbolStack.Push("thing", rp);
    }
}
