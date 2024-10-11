using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// World component to track quest related details
/// </summary>
/// <param name="world"></param>
public class ArchospringVictoryWorldComponent(World world) : WorldComponent(world)
{
    public bool ArchoComponentSeenByPlayer => !ArchoComponentsSeenByPlayer.NullOrEmpty();

    public HashSet<Thing> ArchoComponentsSeenByPlayer = [];

    public Dictionary<ThingDef, bool> BuildingAvailable = new Dictionary<ThingDef, bool>();

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref ArchoComponentsSeenByPlayer, "ArchoComponentsSeenByPlayer", LookMode.Reference);
        Scribe_Collections.Look(ref BuildingAvailable, "BuildingAvailable", LookMode.Def, LookMode.Value);
    }
}
