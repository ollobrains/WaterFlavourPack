using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

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
