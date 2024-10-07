using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_FirstCycle: QuestNode_Root_ArchospringVictory_Cycle
{
    protected override int QuestCycle => 1;
    protected override QuestPart_Activable_ArchoSpringBuilding Activable_ArchoSpringBuilding => new QuestPart_Activable_ArchoSpringBuilding(Thirst_Flavour_PackDefOf.MSS_Thirst_PowerRegulator);

    protected override QuestPartActivable_BuildingUnavailable BuildingFilter => new QuestPartActivable_BuildingUnavailable(Thirst_Flavour_PackDefOf.MSS_Thirst_PowerRegulator);
    protected override ThingDef BuildingDef => Thirst_Flavour_PackDefOf.MSS_Thirst_PowerRegulator;
    protected override SitePartDef CurrentSitePartDef => Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_PowerRegulator_Site;

    protected override bool SpawnSite => true;
    protected override bool SetSuccess => true;
}
