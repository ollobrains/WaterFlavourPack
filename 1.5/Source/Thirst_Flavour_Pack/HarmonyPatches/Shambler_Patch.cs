using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(MutantUtility), nameof(MutantUtility.GetShamblerColor))]
public class ShamblerColorPatch
{
    [HarmonyPostfix]
    public static Color GetShamblerColorPostfix(Color __result)
    {
        Color.RGBToHSV(__result, out float H, out float S, out float V);
        // Change the stauration to make it a light grey
        H = Rand.RangeSeeded(0.08f, 0.12f, __result.GetHashCode());
        S = Rand.RangeSeeded(0.4f, 0.6f, __result.GetHashCode());
        V = Rand.RangeSeeded(0.7f, 1.0f, __result.GetHashCode());
        return Color.HSVToRGB(H, S, V);
    }
}
