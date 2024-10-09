using RimWorld;
using Verse;
// ReSharper disable UnassignedField.Global

namespace Thirst_Flavour_Pack;

[DefOf]
public static class Thirst_Flavour_PackDefOf
{
    public static QuestScriptDef MSS_Thirst_EndGame_WaterVictory;
    public static QuestScriptDef MSS_Thirst_EndGame_WaterVictory_PreCycle;
    public static QuestScriptDef MSS_Thirst_EndGame_WaterVictory_FirstCycle;
    public static QuestScriptDef MSS_Thirst_EndGame_WaterVictory_SecondCycle;
    public static QuestScriptDef MSS_Thirst_EndGame_WaterVictory_ThirdCycle;

    public static IncidentDef MSS_Thirst_GiveQuest_EndGame_ArchospringVictory;

    public static WorldObjectDef MSS_Thirst_Settlement_SecondArchospringCycle;
    public static WorldObjectDef MSS_Thirst_Settlement_ThirdArchospringCycle;

    public static HediffDef MSSThirst_Extracted_Water;


    public static SitePartDef MSS_Thirst_ArchospringSite;
    public static SitePartDef MSS_Thirst_Archospring_PowerRegulator_Site;
    public static SitePartDef MSS_Thirst_Archospring_CatalyticSeparator_Site;
    public static SitePartDef MSS_Thirst_Archospring_SterilizationPlant_Site;

    public static ThingDef MSS_Thirst_ComponentArcho;

    public static ThingDef MSS_Thirst_PowerRegulator;
    public static ThingDef MSS_Thirst_CatalyticSeparator;
    public static ThingDef MSS_Thirst_SterilizationPlant;

    public static SitePartDef MSS_Thirst_BarbCamp;

    static Thirst_Flavour_PackDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(Thirst_Flavour_PackDefOf));
}
