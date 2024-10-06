using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestNode_Root_ArchospringVictory_SecondCycle: QuestNode_Root_ArchospringVictory_Cycle
{
    protected override int QuestCycle => 2;
    protected override QuestPart_Activable_ArchoSpringBuilding Activable_ArchoSpringBuilding => new QuestPart_Activable_ArchoSpringBuilding(Thirst_Flavour_PackDefOf.MSS_CatalyticSeparator, 3);

    protected override QuestPartActivable_BuildingUnavailable BuildingFilter => new QuestPartActivable_BuildingUnavailable(Thirst_Flavour_PackDefOf.MSS_CatalyticSeparator);
    protected override ThingDef BuildingDef => Thirst_Flavour_PackDefOf.MSS_CatalyticSeparator;
    protected override SitePartDef CurrentSitePartDef => Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_CatalyticSeparator_Site;

    protected override bool SpawnSite => true;
    protected override bool SetSuccess => true;
    protected override void RunInt()
    {
        base.RunInt();
        Quest quest = QuestGen.quest;

        // pop up the dialog at the start of the quest
        quest.DialogWithCloseBehavior(
            "[questDescriptionBeforeAccepted]",
            inSignal: quest.AddedSignal,
            signalListMode: QuestPart.SignalListenMode.NotYetAcceptedOnly,
            closeAction: QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd);
    }
}
