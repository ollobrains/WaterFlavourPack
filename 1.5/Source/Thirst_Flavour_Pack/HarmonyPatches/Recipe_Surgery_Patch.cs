using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

/**
 * Helper to grab the base implementation of AvailableOnNow from Recipe_Surgery.
 * Lets us use the base impl instead of the overriden one which may have extra conditions we want to skip.
 */

[HarmonyPatch(typeof(Recipe_Surgery))]
public static class Recipe_Surgery_Patch
{
    [HarmonyPatch(nameof(Recipe_Surgery.AvailableOnNow))]
    [HarmonyReversePatch]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool AvailableOnNow(Recipe_Surgery __instance, Thing thing, BodyPartRecord part)
    {
        throw new NotImplementedException("It's a stub");
    }
}
