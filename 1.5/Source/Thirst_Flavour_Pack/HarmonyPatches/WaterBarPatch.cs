using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(Gene_Hemogen), "BarColor", MethodType.Getter)]
public static class WaterBarPatch
{
    public static void Postfix(ref Color __result)
    {
        __result = new ColorInt(96, 177, 199).ToColor;
    }
}
[HarmonyPatch(typeof(Gene_Hemogen), "BarHighlightColor", MethodType.Getter)]
public static class WaterBarHighlightPatch
{
    public static void Postfix(ref Color __result)
    {
        __result = new ColorInt(0, 177, 199).ToColor;
    }
}
