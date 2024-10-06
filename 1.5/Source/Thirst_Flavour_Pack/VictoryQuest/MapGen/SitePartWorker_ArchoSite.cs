using System.Linq;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class SitePartWorker_ArchoSite: SitePartWorker
{

    public bool MapVisitedSignalSent = false;

    public override void PostMapGenerate(Map map)
    {
        Building_ArchoMachine building = map.listerBuildings.AllColonistBuildingsOfType<Building_ArchoMachine>().FirstOrDefault() ?? map.listerBuildings.allBuildingsNonColonist.OfType<Building_ArchoMachine>().FirstOrDefault();

        if(building is null) return;

        Find.SignalManager.SendSignal(new Signal(building.def.defName + "_MapVisited", true));
    }
}
