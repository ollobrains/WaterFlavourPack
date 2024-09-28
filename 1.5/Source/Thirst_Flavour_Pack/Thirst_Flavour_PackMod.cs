using Verse;
using UnityEngine;
using HarmonyLib;

namespace Thirst_Flavour_Pack;

public class Thirst_Flavour_PackMod : Mod
{
    public static Settings settings;

    public Thirst_Flavour_PackMod(ModContentPack content) : base(content)
    {

        // initialize settings
        settings = GetSettings<Settings>();
#if DEBUG
        ModLog.Log("Thirst_Flavour_PackMod");
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("Feldoh.rimworld.Thirst_Flavour_Pack.main");
        harmony.PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "MSS_Thirst_Settings".Translate();
    }
}
