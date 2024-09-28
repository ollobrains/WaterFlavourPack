using System.Collections.Generic;
using System.Linq;
using PipeSystem;
using RimWorld;
using Thirst_Flavour_Pack.HarmonyPatches;
using Verse;

namespace Thirst_Flavour_Pack.VEF.Incidents;

public class IncidentWorker_WaterContamination : IncidentWorker
{
    public static float SafeLevel = 3;
    protected static PipeNet TargetNet;

    protected override bool CanFireNowSub(IncidentParms parms) => OverSafeCapacity((Map) parms.target);

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        Map map = (Map) parms.target;
        List<Pawn> mapPawnsFreeColonistsSpawned = map.mapPawns.FreeColonistsSpawned;
        CompResourceStorage compResourceStorage = TargetNet?.storages?.MaxBy(s => s.AmountStored);
        if (!OverSafeCapacity(map) || mapPawnsFreeColonistsSpawned.Count <= 0 || compResourceStorage == null)
            return false;

        compResourceStorage.Empty();
        Find.LetterStack.ReceiveLetter("MSS_Thirst_Incident_ContaminationLetterLabel".Translate(),
            "MSS_Thirst_Incident_ContaminationNetLetter".Translate(new NamedArgument(mapPawnsFreeColonistsSpawned.First(), "PAWN")), LetterDefOf.NegativeEvent,
            lookTargets: compResourceStorage.parent);
        WaterContaminationIncident.NextWaterContaminationTick = GenTicks.TicksGame + Thirst_Flavour_PackMod.settings.DaysBetweenWaterDestruction * 60000;
        return true;
    }

    public static bool OverSafeCapacity(Map map)
    {
        TargetNet ??= map?.GetComponent<PipeNetManager>()?.pipeNets?.Find(p => p.def.defName == "VRE_HemogenNet");
        return (TargetNet?.CurrentStored() ?? 0) > SafeLevel;
    }
}
