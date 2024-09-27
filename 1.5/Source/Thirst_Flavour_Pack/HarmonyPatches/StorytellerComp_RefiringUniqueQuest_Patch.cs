using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.HarmonyPatches;

[HarmonyPatch(typeof(StorytellerComp_RefiringUniqueQuest))]
public static class StorytellerComp_RefiringUniqueQuest_Patch
{
    public static int IntervalsPassed => Find.TickManager.TicksGame / 1000;
    public static Lazy<FieldInfo> generateSkipped = new Lazy<FieldInfo>(() => AccessTools.Field(typeof(StorytellerComp_RefiringUniqueQuest), "generateSkipped"));

    [HarmonyPatch(nameof(StorytellerComp_RefiringUniqueQuest.MakeIntervalIncidents))]
    [HarmonyPrefix]
    public static bool MakeIntervalIncidents_Patch(StorytellerComp_RefiringUniqueQuest __instance, ref IEnumerable<FiringIncident> __result, IIncidentTarget target)
    {
        StorytellerCompProperties_RefiringUniqueQuest Props = __instance.props as StorytellerCompProperties_RefiringUniqueQuest;
        IncidentDef incident = Props?.incident;

        if (Props == null || incident is not { defName: "GiveQuest_EndGame_ArchonexusVictory" })
        {
            return true;
        }

        incident = Thirst_Flavour_PackDefOf.MSS_GiveQuest_EndGame_WaterVictory_Pre;
        Props.incident = incident;
        __result = [];

        if (incident.TargetAllowed(target))
        {
            List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
            Quest quest = Enumerable.FirstOrDefault(questsListForReading, t => t.root == incident.questScriptDef);

            IncidentParms parms = __instance.GenerateParms(incident.category, target);

            if (quest != null)
            {
                if (Props.refireEveryDays >= 0.0 && quest.State != QuestState.EndedSuccess && quest.State != QuestState.Ongoing && quest.State != QuestState.NotYetAccepted &&
                    quest.cleanupTick >= 0 && IntervalsPassed == (int) (quest.cleanupTick + Props.refireEveryDays * 60000.0) / 1000)
                {
                    __result = [new FiringIncident(incident, __instance, parms)];
                }
            }
            else
            {
                if (!(bool) generateSkipped.Value.GetValue(__instance)
                        ? IntervalsPassed == (int) (Props.minDaysPassed * 60.0) + 1
                        : GenTicks.TicksGame >= Props.minDaysPassed * 60000.0)
                {
                    __result = [new FiringIncident(incident, __instance, parms)];
                }
            }
        }

        return false;
    }
}
