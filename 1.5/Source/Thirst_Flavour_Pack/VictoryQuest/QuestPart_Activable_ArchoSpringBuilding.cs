using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Once enabled, checks every 60 ticks if the building we're interested in has the correct number of components. Once it does, completes and fires the output signal
/// </summary>
public class QuestPart_Activable_ArchoSpringBuilding(ThingDef def) : QuestPartActivable
{
    public ThingDef Def = def;
    public int NextCheck = 600;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref Def, "Def");
    }

    public override void QuestPartTick()
    {
        if(Find.TickManager.TicksGame < NextCheck)
            return;

        NextCheck = Find.TickManager.TicksGame + 600;

        if (Find.World.GetComponent<ArchospringVictoryWorldComponent>().BuildingComponentCount.GetWithFallback(Def, 0) < Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding)
            return;


        ModLog.Debug($"{Thirst_Flavour_PackMod.settings.ArchotechComponentsToCompleteBuilding} components placed in {def.LabelCap}, moving quest on.");
        Complete();
    }
}
