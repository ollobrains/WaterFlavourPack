using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class Screen_WaterSettlementCinematics: Screen_ArchonexusSettlementCinematics
{
    public Screen_WaterSettlementCinematics(Action cameraJumpAction, Action nextStepAction) : base(cameraJumpAction, nextStepAction)
    {
    }

    Lazy<FieldInfo> cameraJumpAction => new Lazy<FieldInfo>(() => AccessTools.Field(typeof(Screen_WaterSettlementCinematics), "cameraJumpAction"));

    private bool FadeInLatch;

    public override void DoWindowContents(Rect inRect)
    {
        if (!IsFinishedFadingIn())
            return;
        if (!FadeInLatch)
        {
            FadeInLatch = true;
            ((Action) cameraJumpAction.Value.GetValue(this))();
            ScreenFader.SetColor(Color.clear);
        }
        if (IsFinishedDisplayMessage())
        {
            Close(false);
        }
        else
        {
            Rect rect1 = new Rect(0.0f, 0.0f, UI.screenWidth, UI.screenHeight);
            GUI.DrawTexture(rect1, BaseContent.BlackTex);
            Rect rect2 = new Rect(rect1)
            {
                xMin = rect1.center.x - 400f,
                width = 800f,
                yMin = rect1.center.y
            };
            GameFont font = Text.Font;
            int anchor = (int) Text.Anchor;
            Color color = GUI.color;
            Text.Font = GameFont.Medium;
            GUI.color = Color.white;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(inRect), "MSS_Thirst_SoldColonyDescription".Translate());
            Text.Font = font;
            GUI.color = color;
            Text.Anchor = (TextAnchor) anchor;
        }
    }
}
