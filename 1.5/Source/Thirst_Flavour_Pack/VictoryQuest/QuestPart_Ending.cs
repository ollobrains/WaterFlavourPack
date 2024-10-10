using System.Linq;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Very simple questpart that fires the ending screen when receiving in signal
/// </summary>
public class QuestPart_Ending: QuestPart
{
    public string inSignal;
    public bool HasFired = false;

    public override void Notify_QuestSignalReceived(Signal signal)
    {
        if (signal.tag != inSignal || HasFired)
        {
            return;
        }

        if (signal.args.TryGetArg("map", out Map map))
        {
            Building_ArchonexusCore spring = map.listerThings.GetThingsOfType<Building_ArchonexusCore>().FirstOrDefault();

            if (spring != null)
            {
                ModLog.Debug("Firing end screen");
                HasFired = true;
                ArchonexusCountdown.InitiateCountdown(spring);
            }
        }


    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref inSignal, "inSignal");
        Scribe_Values.Look(ref HasFired, "HasFired");
    }
}
