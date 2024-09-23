using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.Comps;

public class CompPowerRegulator: ThingComp
{
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if(respawningAfterLoad)return;
        WaterVictoryWorldComponent.Instance.PowerRegulatorsBuilt++;
    }

    public override void PostDestroy(DestroyMode mode, Map previousMap)
    {
        WaterVictoryWorldComponent.Instance.PowerRegulatorsBuilt--;
    }
}
