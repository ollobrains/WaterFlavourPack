using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack;

[DefOf]
public static class Thirst_Flavour_PackDefOf
{
    // Remember to annotate any Defs that require a DLC as needed e.g.
    // [MayRequireBiotech]
    // public static GeneDef YourPrefix_YourGeneDefName;

    public static QuestScriptDef MSS_EndGame_WaterVictory_FirstCycle;
    public static QuestScriptDef MSS_EndGame_WaterVictory_SecondCycle;
    public static QuestScriptDef MSS_EndGame_WaterVictory_ThirdCycle;

    public static HediffDef MSSThirst_Extracted_Water;


    public static SitePartDef MSS_Thirst_WaterSite;
    static Thirst_Flavour_PackDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(Thirst_Flavour_PackDefOf));
}
