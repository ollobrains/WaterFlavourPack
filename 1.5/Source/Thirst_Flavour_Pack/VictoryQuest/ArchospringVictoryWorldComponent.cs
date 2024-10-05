using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class ArchospringVictoryWorldComponent(World world) : WorldComponent(world)
{
    public static ArchospringVictoryWorldComponent Instance => Find.World.GetComponent<ArchospringVictoryWorldComponent>();

    public bool ArchoComponentSeenByPlayer => !ArchoComponentsSeenByPlayer.NullOrEmpty();

    public List<Thing> ArchoComponentsSeenByPlayer = [];

    public bool FirstCycleRaidQuestFired = false;
    public bool FirstCycleRaidQuestComplete = false;

    public bool SecondFirstCycleRaidQuestFired = false;
    public bool SecondCycleRaidQuestComplete = false;

    public bool ThirdCycleRaidQuestFired = false;
    public bool ThirdCycleRaidQuestComplete = false;


    public int powerRegulatorsBuilt = 0;
    public int PowerRegulatorsBuilt
    {
        get => powerRegulatorsBuilt;
        set
        {
            if (catalyticSeparatorsBuilt == 0 && value == 1)
            {
                LetterMaker.MakeLetter("Victory", "Victory", LetterDefOf.PositiveEvent);
            }
            powerRegulatorsBuilt = Math.Max(0, value);
        }
    }

    public int catalyticSeparatorsBuilt = 0;
    public int CatalyticSeparatorsBuilt
    {
        get => catalyticSeparatorsBuilt;
        set
        {
            catalyticSeparatorsBuilt = Math.Max(0, value);
        }
    }

    public int sterilizationPlantsBuilt = 0;
    public int SterilizationPlantsBuilt
    {
        get => sterilizationPlantsBuilt;
        set
        {
            sterilizationPlantsBuilt = Math.Max(0, value);
        }
    }


    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref powerRegulatorsBuilt, "PowerRegulatorsBuilt", 0);
        Scribe_Values.Look(ref catalyticSeparatorsBuilt, "CatalyticSeparatorsBuilt", 0);
        Scribe_Values.Look(ref sterilizationPlantsBuilt, "SterilizationPlantsBuilt", 0);
        Scribe_Collections.Look(ref ArchoComponentsSeenByPlayer, "ArchoComponentsSeenByPlayer", LookMode.Reference);
    }
}
