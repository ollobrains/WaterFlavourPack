using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using VanillaRacesExpandedSanguophage;
using Verse;

namespace Thirst_Flavour_Pack.VEF.HarmonyPatches;

[HarmonyPatch(typeof(CompDraincasket))]
public static class CompDraincasket_Patch
{
    public static Lazy<FieldInfo> contentsKnown = new Lazy<FieldInfo>(() => AccessTools.Field(typeof(CompDraincasket), "contentsKnown"));

    [HarmonyPatch(nameof(CompDraincasket.CompTick))]
    [HarmonyPostfix]
    public static void CompTick_Patch(CompDraincasket __instance)
    {
        bool shouldEject = false;
        if (__instance.parent.IsHashIntervalTick(Thirst_Flavour_PackMod.settings.ThirstCasketHediffTickRate))
        {
            List<Pawn> pawns = __instance.innerContainer.OfType<Pawn>().ToList();
            foreach (Pawn pawn in pawns)
            {
                Hediff hediff = pawn.health.GetOrAddHediff(Thirst_Flavour_PackDefOf.MSSThirst_Extracted_Water);
                hediff.Severity += 0.001f;

                if (Mathf.Approximately(hediff.Severity, 1f))
                {
                    shouldEject = true;
                }

                // InsertPawn was inlined, so do this here
                Gene_Hemogen gene = (Gene_Hemogen)pawn.genes.GetGene(GeneDefOf.Hemogenic);
                if(gene != null)
                    GeneResourceDrainUtility.OffsetResource(gene, -1f);
            }
        }

        if (!shouldEject)
        {
            return;
        }

        // Don't change the list during iteration
        foreach (Pawn pawn in __instance.innerContainer.OfType<Pawn>())
        {
            PawnComponentsUtility.AddComponentsForSpawn(pawn);
            pawn.filth.GainFilth(ThingDefOf.Filth_Slime);
            Hediff hediff = pawn.health.GetOrAddHediff(Thirst_Flavour_PackDefOf.MSSThirst_Extracted_Water);
            pawn.Kill(new DamageInfo?(), hediff);
        }
        __instance.innerContainer.TryDropAll(__instance.parent.InteractionCell, __instance.parent.Map, ThingPlaceMode.Near);
        contentsKnown.Value.SetValue(__instance, true);
    }

    [HarmonyPatch(nameof(CompDraincasket.EjectContents))]
    [HarmonyPrefix]
    public static void EjectContents_Patch(CompDraincasket __instance)
    {
        foreach (Pawn pawn in __instance.innerContainer.OfType<Pawn>())
        {
            PawnComponentsUtility.AddComponentsForSpawn(pawn);
            pawn.filth.GainFilth(ThingDefOf.Filth_Slime);
            if (pawn.RaceProps.IsFlesh)
            {
                pawn.health.AddHediff(InternalDefOf.VRE_DraincasketSickness);
                Hediff hediff = pawn.health.GetOrAddHediff(Thirst_Flavour_PackDefOf.MSSThirst_Extracted_Water);
                hediff.Severity += 0.1f;
            }
        }
        __instance.innerContainer.TryDropAll(__instance.parent.InteractionCell, __instance.parent.Map, ThingPlaceMode.Near);
        contentsKnown.Value.SetValue(__instance, true);
    }
}
