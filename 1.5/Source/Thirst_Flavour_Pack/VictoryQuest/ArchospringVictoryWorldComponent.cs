using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class ArchospringVictoryWorldComponent(World world) : WorldComponent(world)
{
    public bool ArchoComponentSeenByPlayer => !ArchoComponentsSeenByPlayer.NullOrEmpty();

    public List<Thing> ArchoComponentsSeenByPlayer = [];


    public Dictionary<ThingDef, int> BuildingComponentCount = new Dictionary<ThingDef, int>();
    public Dictionary<ThingDef, bool> BuildingAvailable = new Dictionary<ThingDef, bool>();

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref ArchoComponentsSeenByPlayer, "ArchoComponentsSeenByPlayer", LookMode.Reference);
        Scribe_Collections.Look(ref BuildingComponentCount, "BuildingComponentCount", LookMode.Def, LookMode.Value);
        Scribe_Collections.Look(ref BuildingAvailable, "BuildingAvailable", LookMode.Def, LookMode.Value);
    }
}
