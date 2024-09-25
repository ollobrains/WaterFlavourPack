using Thirst_Flavour_Pack.HarmonyPatches;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack;

public class Settings : ModSettings
{
    public int ThirstCasketHediffTickRate = 360;
    public int MaxSafeWaterInNet = 30;
    public int SafeWaterPacks = 10;
    public int MaxUnsafeWaterPacks = 30;
    public int DaysBetweenWaterDestruction = 10;
    public int BadWaterNoticingRollsPerDay = 2;

    private string[] buffers = new string[32];
    private int bufferIndex = 0;

    public int WaterQuestBuildingsRequiredFirstCycle = 3;
    public int WaterQuestBuildingsRequiredSecondCycle = 3;
    public int WaterQuestBuildingsRequiredThirdCycle = 3;

    public int WaterQuestColonistsAllowed = 10;
    public int WaterQuestAnimalsAllowed = 10;
    public int WaterQuestItemsAllowed = 40;
    public int WaterQuestRelicsAllowed = 10;

    public void DoWindowContents(Rect wrect)
    {
        bufferIndex = 0;
        Listing_Standard options = new Listing_Standard();
        options.Begin(wrect);

        options.Label("MSS_Thirst_WaterQuestColonistsAllowed".Translate(WaterQuestColonistsAllowed));
        options.IntAdjuster(ref WaterQuestColonistsAllowed, 1, 1);

        options.Label("MSS_Thirst_WaterQuestAnimalsAllowed".Translate(WaterQuestAnimalsAllowed));
        options.IntAdjuster(ref WaterQuestAnimalsAllowed, 1, 1);

        options.Label("MSS_Thirst_WaterQuestItemsAllowed".Translate(WaterQuestItemsAllowed));
        options.IntAdjuster(ref WaterQuestItemsAllowed, 1, 1);

        options.Label("MSS_Thirst_WaterQuestRelicsAllowed".Translate(WaterQuestRelicsAllowed));
        options.IntAdjuster(ref WaterQuestRelicsAllowed, 1, 1);
        options.CheckboxLabeled("Thirst Flavour Pack_Settings_SettingName".Translate(), ref setting);

        options.Label("MSS_Thirst_ThirstCasketHediffTickRate".Translate(ThirstCasketHediffTickRate));
        options.IntEntry(ref ThirstCasketHediffTickRate, ref buffers[bufferIndex++], 1);
        if (ThirstCasketHediffTickRate < 0) ThirstCasketHediffTickRate = 0;

        options.Label("MSS_Thirst_Settings_MaxSafeWaterInNet".Translate());
        options.IntEntry(ref MaxSafeWaterInNet, ref buffers[bufferIndex++], 1);
        if (MaxSafeWaterInNet < 1) MaxSafeWaterInNet = 1;

        options.Label("MSS_Thirst_Settings_SafeWaterPacks".Translate());
        options.IntEntry(ref SafeWaterPacks, ref buffers[bufferIndex++], 1);
        if (SafeWaterPacks < 1) SafeWaterPacks = 1;

        options.Label("MSS_Thirst_Settings_MaxUnsafeWaterPacks".Translate());
        options.IntEntry(ref MaxUnsafeWaterPacks, ref buffers[bufferIndex++], 1);
        if (MaxUnsafeWaterPacks < 1) MaxUnsafeWaterPacks = 1;

        options.Label("MSS_Thirst_Settings_DaysBetweenWaterDestruction".Translate());
        options.IntEntry(ref DaysBetweenWaterDestruction, ref buffers[bufferIndex++], 1);
        if (DaysBetweenWaterDestruction < 1) DaysBetweenWaterDestruction = 1;

        options.Label("MSS_Thirst_Settings_BadWaterNoticingRollsPerDay".Translate());
        options.IntEntry(ref BadWaterNoticingRollsPerDay, ref buffers[bufferIndex++], 1);
        if (BadWaterNoticingRollsPerDay < 1) BadWaterNoticingRollsPerDay = 1;

        WaterContaminationIncident.WaterItemContaminationCurveReset = true;

        options.Gap();

        options.End();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref WaterQuestColonistsAllowed, "WaterQuestColonistsAllowed", 10);
        Scribe_Values.Look(ref WaterQuestAnimalsAllowed, "WaterQuestAnimalsAllowed", 10);
        Scribe_Values.Look(ref WaterQuestItemsAllowed, "WaterQuestItemsAllowed", 40);
        Scribe_Values.Look(ref WaterQuestRelicsAllowed, "WaterQuestRelicsAllowed", 10);
        Scribe_Values.Look(ref ThirstCasketHediffTickRate, "ThirstCasketHediffTickRate", 360);
        Scribe_Values.Look(ref MaxSafeWaterInNet, "MaxSafeWaterInNet", 30);
        Scribe_Values.Look(ref SafeWaterPacks, "SafeWaterPacks", 10);
        Scribe_Values.Look(ref MaxUnsafeWaterPacks, "MaxUnsafeWaterPacks", 30);
        Scribe_Values.Look(ref DaysBetweenWaterDestruction, "DaysBetweenWaterDestruction", 10);
        Scribe_Values.Look(ref BadWaterNoticingRollsPerDay, "BadWaterNoticingRollsPerDay", 2);
    }
}
