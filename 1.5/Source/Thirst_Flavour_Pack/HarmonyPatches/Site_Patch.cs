using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Thirst_Flavour_Pack.VictoryQuest;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(Site))]
public static class Site_Patch
{
    public static HashSet<SitePartDef> SitesToNotDespawn = [
        Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_PowerRegulator_Site,
        Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_CatalyticSeparator_Site,
        Thirst_Flavour_PackDefOf.MSS_Thirst_Archospring_SterilizationPlant_Site
    ];

    [HarmonyPatch(nameof(Site.ShouldRemoveMapNow))]
    [HarmonyPostfix]
    public static void ShouldRemoveMapNow_Postfix(Site __instance, ref bool __result)
    {
        if (__instance.parts.Any(p => p.def == Thirst_Flavour_PackDefOf.MSS_Thirst_ArchospringSite))
        {
            __result = false;
            return;
        }

        // Stops our sites being auto-removed when caravanning away
        if (__instance.parts.Any(p => SitesToNotDespawn.Contains(p.def)))
        {
            // Do auto-remove when complete.
            Building_ArchoMachine bld = __instance.Map.listerThings.GetThingsOfType<Building_ArchoMachine>().FirstOrDefault();
            if (bld != null && bld.IsComplete() && !__instance.Map.mapPawns.AnyColonistSpawned && !__instance.Map.PlayerPawnsForStoryteller.Any())
            {
                __result = true;
                return;
            }
            __result = false;
        }
    }

}
