using System;
using HarmonyLib;
using VanillaRacesExpandedSanguophage;
using System.Linq;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.VEF.HarmonyPatches;

[HarmonyPatch(typeof(CompDraincasket))]
public static class CompDraincasket_Patch
{
    [HarmonyPatch(nameof(CompDraincasket.CompTick))]
    [HarmonyPostfix]
    public static void CompTick_Patch(CompDraincasket __instance)
    {
        bool shouldEject = false;
        if (__instance.parent.IsHashIntervalTick(Thirst_Flavour_PackMod.settings.ThirstCasketHediffTickRate))
        {
            foreach (Pawn pawn in __instance.innerContainer.OfType<Pawn>())
            {
                Hediff hediff = pawn.health.GetOrAddHediff(Thirst_Flavour_PackDefOf.MSSThirst_Extracted_Water);
                hediff.Severity += 0.001f;

                if (Mathf.Approximately(hediff.Severity, 1f))
                {
                    shouldEject = true;
                }
            }
        }

        // Don't change the list during iteration
        if (shouldEject)
        {
            __instance.EjectContents(__instance.parent.Map);
        }
    }

}
