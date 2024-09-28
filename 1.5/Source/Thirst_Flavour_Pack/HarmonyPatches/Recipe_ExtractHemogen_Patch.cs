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
        AccessTools.Method(typeof(Recipe_ExtractHemogen), "PawnHasEnoughBloodForExtraction"));

    public static readonly Lazy<MethodInfo> OnSurgerySuccess =
        new Lazy<MethodInfo>(() => AccessTools.Method(typeof(Recipe_ExtractHemogen), "OnSurgerySuccess"));

    public static readonly Lazy<MethodInfo> ReportViolation =
        new Lazy<MethodInfo>(() => AccessTools.Method(typeof(Recipe_ExtractHemogen), "ReportViolation"));

    [HarmonyPatch(nameof(Recipe_ExtractHemogen.AvailableOnNow))]
    [HarmonyPrefix]
    public static bool Recipe_ExtractHemogen_AvailableOnNow_Patch(Recipe_ExtractHemogen __instance, Thing thing, BodyPartRecord part, ref bool __result)
    {
        __result = true;
        // Don't prevent extract hemogen on hemogenic pawns
        if (thing is not Pawn pawn || !Recipe_Surgery_Patch.AvailableOnNow(__instance, thing, part))
        {
            return false;
        }

        // Don't allow extraction when they've been extracted
        if (pawn.health.hediffSet.GetFirstHediffOfDef(Thirst_Flavour_PackDefOf.MSSThirst_Extracted_Water) != null)
        {
            __result = false;
        }

        return false;
    }

    [HarmonyPatch("AvailableReport")]
    [HarmonyPrefix]
    public static bool AvailableReport_Patch(Recipe_ExtractHemogen __instance, ref AcceptanceReport __result, Thing thing, BodyPartRecord part = null)
    {
        if (thing is Pawn pawn)
        {
            if (pawn.DevelopmentalStage.Baby())
            {
                __result = "TooSmall".Translate();
                return false;
            }
            if (pawn.health.hediffSet.GetFirstHediffOfDef(Thirst_Flavour_PackDefOf.MSSThirst_Extracted_Water) != null)
            {
                __result = "MSS_Thirst_HasForcedDehydration".Translate();
                return false;
            }
        }

        __result = __instance.AvailableOnNow(thing, part);

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
                Messages.Message( "MessagePawnHadNotEnoughBloodToProduceHemogenPack".Translate(pawn.Named("PAWN")), (LookTargets) (Thing) pawn, MessageTypeDefOf.NeutralEvent);
            }
            else
            {
                Hediff hediff = HediffMaker.MakeHediff(Thirst_Flavour_PackDefOf.MSSThirst_Extracted_Water, pawn);
                hediff.Severity = 0.45f;
                pawn.health.AddHediff(hediff);
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
