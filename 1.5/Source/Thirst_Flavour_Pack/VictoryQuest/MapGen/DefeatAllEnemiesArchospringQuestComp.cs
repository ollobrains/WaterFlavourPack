using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

/// <summary>
/// WorldObjectComp to monitor the archospring map for all enemies defeated
/// </summary>
public class DefeatAllEnemiesArchospringQuestComp : WorldObjectComp
{
    public static readonly string AllEnemiesDefeated_Archospring_Signal = "MSS_Thirst_AllEnemiesDefeated_Archospring";
    private bool active;

    public bool Active => active;

    public void StartQuest()
    {
      StopQuest();
      active = true;
    }

    public void StopQuest()
    {
      active = false;
    }

    public override void CompTick()
    {
      base.CompTick();
      if (!active || parent is not MapParent mParent)
        return;
      CheckAllEnemiesDefeated(mParent);
    }

    private void CheckAllEnemiesDefeated(MapParent mapParent)
    {
      if (!mapParent.HasMap || GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true))
        return;
      SignalArgs args = new SignalArgs();
      // Send the current map as a signal arg so that the archospring building can be grabbed
      args.Add(new NamedArgument(mapParent.Map, "map"));
      Find.SignalManager.SendSignal(new Signal(AllEnemiesDefeated_Archospring_Signal, args, true));
      StopQuest();
    }

    public override void PostExposeData()
    {
      base.PostExposeData();
      Scribe_Values.Look(ref active, "active");
    }

    public override string CompInspectStringExtra()
    {
        return active ? "MSS_Thirst_ArchospringSite".Translate().CapitalizeFirst() : null;
    }
}
