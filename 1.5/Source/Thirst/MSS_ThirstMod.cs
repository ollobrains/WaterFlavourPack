using System.Reflection;
using Verse;
using UnityEngine;
using HarmonyLib;
using RimWorld;

namespace MSS_Thirst;

public class MSS_ThirstMod : Mod
{
    public MSS_ThirstMod(ModContentPack content) : base(content)
    {
        Log.Message("Hello world from MSS_Thirst");
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("MSS.Thirst.main");	
        harmony.PatchAll();
        
        // FieldInfo BarTex = Assembly.GetAssembly(typeof(GeneGizmo_ResourceHemogen))
        //     .GetType("RimWorld.GeneGizmo_ResourceHemogen")
        //     .GetField("BarTex", BindingFlags.NonPublic | BindingFlags.Static);
        // BarTex?.SetValue(null, SolidColorMaterials.NewSolidColorTexture(new Color(0f, 0.6f, 1)));
        
        // FieldInfo barTex = AccessTools.Field(typeof(GeneGizmo_ResourceHemogen), "barTex");
        //
        // barTex?.SetValue(null, SolidColorMaterials.NewSolidColorTexture(new Color(0f, 0.6f, 1)));
    }
}
