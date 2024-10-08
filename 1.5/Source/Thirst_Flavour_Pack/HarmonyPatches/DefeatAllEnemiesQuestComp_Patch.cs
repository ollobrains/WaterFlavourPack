using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(DefeatAllEnemiesQuestComp))]
public static class DefeatAllEnemiesQuestComp_Patch
{
    public static Lazy<FieldInfo> DefeatAllEnemiesQuestComp_active = new Lazy<FieldInfo>(() => AccessTools.Field(typeof(DefeatAllEnemiesQuestComp), "active"));

    [HarmonyPatch("GiveRewardsAndSendLetter")]
    [HarmonyPrefix]
    public static bool GiveRewardsAndSendLetter_Patch(DefeatAllEnemiesQuestComp __instance)
    {
        if (__instance.requestingFaction == Faction.OfPlayer && __instance.rewards.NullOrEmpty())
        {
            return false;
        }

        return true;
    }

    [HarmonyPatch("CompInspectStringExtra")]
    [HarmonyPrefix]
    public static bool CompInspectStringExtra_Patch(DefeatAllEnemiesQuestComp __instance, ref string __result)
    {
        if (__instance.requestingFaction == Faction.OfPlayer && __instance.rewards.NullOrEmpty())
        {
            __result = (bool)DefeatAllEnemiesQuestComp_active.Value.GetValue(__instance) ? "MSS_Thirst_ArchospringSite".Translate().CapitalizeFirst() : null;
            return false;
        }

        return true;
    }

}
