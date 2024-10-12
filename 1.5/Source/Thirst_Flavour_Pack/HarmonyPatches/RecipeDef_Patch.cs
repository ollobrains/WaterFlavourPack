using HarmonyLib;
using Thirst_Flavour_Pack.VictoryQuest;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(RecipeDef))]
public static class RecipeDef_Patch
{
    [HarmonyPatch(nameof(RecipeDef.AvailableNow), MethodType.Getter)]
    [HarmonyPrefix]
    public static bool RecipeDef_AvailableNow_Patch(RecipeDef __instance, ref bool __result)
    {
        if (__instance == Thirst_Flavour_PackDefOf.MSS_Thirst_Make_ComponentArcho && (!Find.World.GetComponent<ArchospringVictoryWorldComponent>()?.ArchoComponentSeenByPlayer ?? false))
        {
            __result = false;
            return false;
        }

        return true;
    }
}
