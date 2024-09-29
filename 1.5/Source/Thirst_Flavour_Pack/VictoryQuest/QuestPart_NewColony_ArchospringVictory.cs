using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_NewColony_ArchospringVictory: QuestPart_NewColony
{
    public Lazy<MethodInfo> TileChosen = new Lazy<MethodInfo>(() => AccessTools.Method(typeof(QuestPart_NewColony), "TileChosen"));
    public void NewPostThingsSelected(List<Thing> allThings)
    {
        Find.WindowStack.Add(new Screen_Archospring_SettlementCinematics((Action) (() => CameraJumper.TryJump(CameraJumper.GetWorldTarget((GlobalTargetInfo) allThings.First(t => t is Pawn)))), (Action) (() =>
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

    public override void Notify_QuestSignalReceived(Signal signal)
    {
        if (signal.tag != inSignal)
            return;
        Find.MainTabsRoot.EscapeCurrentTab(false);
        Find.World.renderer.RegenerateLayersIfDirtyInLongEvent();
        Find.WindowStack.Add(new Dialog_ChooseThingsForNewColony_ArchospringQuest(NewPostThingsSelected,
            maxColonists: Thirst_Flavour_PackMod.settings.WaterQuestColonistsAllowed,
            maxAnimals: Thirst_Flavour_PackMod.settings.WaterQuestAnimalsAllowed,
            maxItems: Thirst_Flavour_PackMod.settings.WaterQuestItemsAllowed,
            maxRelics: Thirst_Flavour_PackMod.settings.WaterQuestRelicsAllowed, cancel: (Action) (() =>
        {
            if (outSignalCancelled.NullOrEmpty())
                return;
            Find.SignalManager.SendSignal(new Signal(outSignalCancelled, false));
        })));
    }
}
