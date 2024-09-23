﻿using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.Comps;

public class CompSterilizationPlant: ThingComp
{
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if(respawningAfterLoad)return;
        WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt++;
    }

    public override void PostDestroy(DestroyMode mode, Map previousMap)
    {
        WaterVictoryWorldComponent.Instance.SterilizationPlantsBuilt--;
    }
}
