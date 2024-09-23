using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.Comps;

public class CompCatalyticSeparator: ThingComp
{
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if(respawningAfterLoad)return;
        WaterVictoryWorldComponent.Instance.CatalyticSeparatorsBuilt++;
    }

    public override void PostDestroy(DestroyMode mode, Map previousMap)
    {
        WaterVictoryWorldComponent.Instance.CatalyticSeparatorsBuilt--;
    }
}
