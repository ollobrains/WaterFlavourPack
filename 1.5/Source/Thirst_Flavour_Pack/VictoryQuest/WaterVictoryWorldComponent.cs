using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class WaterVictoryWorldComponent: WorldComponent
{
    public static WaterVictoryWorldComponent Instance;
    public WaterVictoryWorldComponent(World world) : base(world)
    {
        Instance = this;
    }

    public int PowerRegulatorsBuilt = 0;
    public int CatalyticSeparatorsBuilt = 0;
    public int SterilizationPlantsBuilt = 0;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref PowerRegulatorsBuilt, "PowerRegulatorsBuilt", 0);
        Scribe_Values.Look(ref CatalyticSeparatorsBuilt, "CatalyticSeparatorsBuilt", 0);
        Scribe_Values.Look(ref SterilizationPlantsBuilt, "SterilizationPlantsBuilt", 0);
    }
}
