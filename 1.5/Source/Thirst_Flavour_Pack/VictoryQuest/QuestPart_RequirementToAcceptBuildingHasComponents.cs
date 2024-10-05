using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptBuildingHasComponents(ThingDef bld): QuestPart_RequirementsToAccept
{
    public ThingDef Building = bld;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref Building, "Building");
    }

    public bool IsAvailable => Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingAvailable.GetWithFallback(Building, false);

    public override AcceptanceReport CanAccept()
    {
        return !IsAvailable ? new AcceptanceReport("MSS_Thirst_BuildingRequired".Translate(Building.LabelCap)) : AcceptanceReport.WasAccepted;
    }
}
