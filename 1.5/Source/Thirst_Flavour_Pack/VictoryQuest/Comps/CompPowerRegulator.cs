using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.Comps;

public class CompPowerRegulator: ThingComp
{
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if(respawningAfterLoad)return;
        ArchospringVictoryWorldComponent.Instance.PowerRegulatorsBuilt++;
    }

    public override void PostDestroy(DestroyMode mode, Map previousMap)
    {
        ArchospringVictoryWorldComponent.Instance.PowerRegulatorsBuilt--;
    }
}
