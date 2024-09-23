using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

[HarmonyPatch(typeof(QuestPart_NewColony))]
public static class QuestPart_NewColony_Patch
{
    [HarmonyPatch("PostThingsSelected")]
    [HarmonyPrefix]
    public static bool PostThingsSelected(QuestPart_NewColony __instance, List<Thing> allThings)
    {
        if (__instance is QuestPart_NewColonyWater water)
        {
            water.NewPostThingsSelected(allThings);
            return false;
        }

        return true;
    }
}

public class QuestPart_NewColonyWater: QuestPart_NewColony
{
    public Lazy<MethodInfo> TileChosen = new Lazy<MethodInfo>(() => AccessTools.Method(typeof(QuestPart_NewColony), "TileChosen"));
    public void NewPostThingsSelected(List<Thing> allThings)
    {
        Find.WindowStack.Add(new Screen_WaterSettlementCinematics((Action) (() => CameraJumper.TryJump(CameraJumper.GetWorldTarget((GlobalTargetInfo) allThings.First(t => t is Pawn)))), (Action) (() =>
        {
            MoveColonyUtility.PickNewColonyTile(choseTile => TileChosen.Value.Invoke(this, [choseTile, allThings]), (Action) (() =>
            {
                if (outSignalCancelled.NullOrEmpty())
                    return;
                Find.SignalManager.SendSignal(new Signal(outSignalCancelled, false));
            }));
            ScreenFader.StartFade(Color.clear, 2f);
        })));
    }
}
