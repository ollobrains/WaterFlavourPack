using HarmonyLib;
using RimWorld;
using Thirst_Flavour_Pack.VictoryQuest;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(ThingStyleHelper))]
public static class ThingStyleHelper_Patch
{
    [HarmonyPatch(nameof(ThingStyleHelper.SetEverSeenByPlayer))]
    [HarmonyPostfix]
    public static void SetEverSeenByPlayer_Patch(Thing thing, bool everSeenByPlayer)
    {
        if (everSeenByPlayer && thing is Thing_ComponentArcho)
        {
            Find.World.GetComponent<ArchospringVictoryWorldComponent>().ArchoComponentSeenByPlayer = true;
        }
    }

}
