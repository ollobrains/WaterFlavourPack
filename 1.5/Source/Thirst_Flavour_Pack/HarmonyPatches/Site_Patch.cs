using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(Site))]
public static class Site_Patch
{
    public static HashSet<SitePartDef> SitesToNotDespawn = [
        Thirst_Flavour_PackDefOf.MSS_Thirst_ArchospringSite,
        Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_PowerRegulator_Site,
        Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_CatalyticSeparator_Site,
        Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_SterilizationPlant_Site
    ];

    [HarmonyPatch(nameof(Site.ShouldRemoveMapNow))]
    [HarmonyPostfix]
    public static void ShouldRemoveMapNow_Postfix(Site __instance, ref bool __result)
    {
        // Stops our sites being auto-removed when caravanning away
        if (__instance.parts.Any(p => SitesToNotDespawn.Contains(p.def)))
        {
            __result = false;
        }
    }

}
