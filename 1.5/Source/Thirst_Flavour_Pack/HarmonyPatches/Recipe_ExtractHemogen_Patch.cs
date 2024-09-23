using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(Recipe_ExtractHemogen))]
public static class Recipe_ExtractHemogen_Patch
{
    public static readonly Lazy<MethodInfo> PawnHasEnoughBloodForExtraction = new Lazy<MethodInfo>(() =>
        AccessTools.Method(typeof(Recipe_ExtractHemogen), "HasEnoughBloodForExtraction"));

    public static readonly Lazy<MethodInfo> OnSurgerySuccess =
        new Lazy<MethodInfo>(() => AccessTools.Method(typeof(Recipe_ExtractHemogen), "OnSurgerySuccess"));

    public static readonly Lazy<MethodInfo> ReportViolation =
        new Lazy<MethodInfo>(() => AccessTools.Method(typeof(Recipe_ExtractHemogen), "ReportViolation"));

    [HarmonyPatch(nameof(Recipe_ExtractHemogen.AvailableOnNow))]
    [HarmonyPrefix]
    public static bool Recipe_ExtractHemogen_AvailableOnNow_Patch(Recipe_ExtractHemogen __instance, Thing thing, BodyPartRecord part, ref bool __result)
    {
        // Don't prevent extract hemogen on hemogenic pawns
        __result = (thing is Pawn pawn ? pawn.genes : null) == null || Recipe_Surgery_Patch.AvailableOnNow(__instance, thing, part);
        return false;
    }

    [HarmonyPatch(nameof(Recipe_ExtractHemogen.ApplyOnPawn))]
    [HarmonyPrefix]
    public static bool Recipe_ExtractHemogen_ApplyOnPawn_Patch(Recipe_ExtractHemogen __instance, Pawn pawn,
        BodyPartRecord part,
        Pawn billDoer,
        List<Thing> ingredients,
        Bill bill)
    {
        // Don't apply bloodloss on extract hemogen
        if (ModLister.CheckBiotech("Hemogen extraction"))
        {
            if (!(bool)PawnHasEnoughBloodForExtraction.Value.Invoke(__instance, [pawn]))
            {
                Messages.Message((string) "MessagePawnHadNotEnoughBloodToProduceHemogenPack".Translate(pawn.Named("PAWN")), (LookTargets) (Thing) pawn, MessageTypeDefOf.NeutralEvent);
            }
            else
            {
                OnSurgerySuccess.Value.Invoke(__instance, [pawn, part, billDoer, ingredients, bill]);
                if (__instance.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
                {
                    ReportViolation.Value.Invoke(__instance, [pawn, billDoer, pawn.HomeFaction, -1, HistoryEventDefOf.ExtractedHemogenPack]);
                }
            }
        }

        return false;
    }
}
