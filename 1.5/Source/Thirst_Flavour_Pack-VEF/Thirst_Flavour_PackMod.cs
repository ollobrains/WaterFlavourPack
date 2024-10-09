using HarmonyLib;
using Verse;

namespace Thirst_Flavour_Pack.VEF;

public class Thirst_Flavour_Pack_VEF_Mod : Mod
{
    public Thirst_Flavour_Pack_VEF_Mod(ModContentPack content) : base(content)
    {
#if DEBUG
        ModLog.Log("Thirst_Flavour_Pack_VEF_Mod");
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("Feldoh.rimworld.Thirst_Flavour_Pack.VEF.main");
        harmony.PatchAll();
    }
}
