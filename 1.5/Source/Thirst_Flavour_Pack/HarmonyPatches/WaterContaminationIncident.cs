using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(IngestionOutcomeDoer_OffsetHemogen), "DoIngestionOutcomeSpecial")]
public class WaterContaminationIncident
{
    public static int NextWaterContaminationTick = 60000 * 10;
    public static bool WaterItemContaminationCurveReset = true;
    private static SimpleCurve _waterItemContaminationCurve = null;

    public static SimpleCurve WaterItemContaminationCurve
    {
        get
        {
            if (WaterItemContaminationCurveReset || _waterItemContaminationCurve == null)
            {
                _waterItemContaminationCurve = new SimpleCurve(
                [
                    new CurvePoint(0, 0),
                    new CurvePoint(Thirst_Flavour_PackMod.settings.SafeWaterPacks, 0),
                    new CurvePoint(
                        Thirst_Flavour_PackMod.settings.SafeWaterPacks +
                        (Thirst_Flavour_PackMod.settings.MaxUnsafeWaterPacks - Thirst_Flavour_PackMod.settings.SafeWaterPacks / 2f), 0.2f),
                    new CurvePoint(Thirst_Flavour_PackMod.settings.MaxUnsafeWaterPacks, 0.9f)
                ]);
            }

            return _waterItemContaminationCurve;
        }
    }


    public static void Postfix(IngestionOutcomeDoer_OffsetHemogen __instance, Pawn pawn, Thing ingested)
    {
        if (!(__instance.offset > 0f) || GenTicks.TicksGame <= NextWaterContaminationTick) return;
        NextWaterContaminationTick = GenTicks.TicksGame + 60000 / Thirst_Flavour_PackMod.settings.BadWaterNoticingRollsPerDay; // Don't check every time just a few times a day

        List<Thing> things = Find.CurrentMap.listerThings.ThingsOfDef(ingested.def);
        float chance = WaterItemContaminationCurve.Evaluate(things.Select(t => t.stackCount).Sum());
        ModLog.Debug($"Water contamination chance: {chance} for {things.Count} items");
        if (!Rand.Chance(chance)) return;

        List<Thing> toDestroy = things.TakeRandom(Mathf.Clamp(Mathf.CeilToInt(things.Count * chance), 1, things.Count - 1)).ToList();
        int destroyedCount = 0;
        foreach (Thing thing in toDestroy)
        {
            destroyedCount += thing.stackCount;
            thing.Destroy(DestroyMode.Vanish);
        }

        NextWaterContaminationTick = GenTicks.TicksGame + 60000 * Thirst_Flavour_PackMod.settings.DaysBetweenWaterDestruction;

        Find.LetterStack.ReceiveLetter("MSS_Thirst_Incident_ContaminationLetterLabel".Translate(),
            "MSS_Thirst_Incident_ContaminationLetter".Translate(new NamedArgument(pawn, "PAWN"), new NamedArgument(ingested, "ITEM"),
                new NamedArgument(destroyedCount, "COUNT")), LetterDefOf.NegativeEvent);
    }
}
