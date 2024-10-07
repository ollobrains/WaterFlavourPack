using Thirst_Flavour_Pack.HarmonyPatches;
using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack;

public class Settings : ModSettings
{
    public Vector2 optionsScrollPosition;

    public int ThirstCasketHediffTickRate = 360;
    public int MaxSafeWaterInNet = 30;
    public int SafeWaterPacks = 10;
    public int MaxUnsafeWaterPacks = 30;
    public int DaysBetweenWaterDestruction = 10;
    public int BadWaterNoticingRollsPerDay = 2;

    private string[] buffers = new string[32];
    private int bufferIndex = 0;

    public int ArchotechComponentsToCompleteBuilding = 3;

    public IntRange ArchoQuestComponentHuntInterval = new IntRange(3600, 108000); // default 1 min to 30 mins
    public IntRange ArchoQuestSubQuestInterval = new IntRange(3600, 108000); // default 1 min to 30 mins

    public void DoWindowContents(Rect wrect)
    {
        int labels = 9;
        int intAdjuster = 6;
        int intEntries = 6;
        int intRanges = 2;
        int gaps = 3;

        float optionsViewRectHeight = (labels * 21.3333333f) + (intAdjuster * 24f) + (intEntries * 24f) + (intRanges * 32f) + (gaps * 12f) + 30f;
        bool willHaveScrollbar = optionsViewRectHeight > wrect.height;
        Rect viewRect = new Rect(wrect.x, wrect.y, wrect.width -  (willHaveScrollbar ? 26f : 0f), optionsViewRectHeight);
        Widgets.BeginScrollView(wrect, ref optionsScrollPosition, viewRect);

        bufferIndex = 0;
        Listing_Standard options = new Listing_Standard();
        options.Begin(viewRect);

        options.Label("MSS_Thirst_ThirstCasketHediffTickRate".Translate(ThirstCasketHediffTickRate));
        options.IntEntry(ref ThirstCasketHediffTickRate, ref buffers[bufferIndex++], 1);
        if (ThirstCasketHediffTickRate < 0) ThirstCasketHediffTickRate = 0;

        options.Gap();

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

        options.Gap();

        options.Label("".Translate(ArchotechComponentsToCompleteBuilding));
        options.IntAdjuster(ref ArchotechComponentsToCompleteBuilding, 1);

        options.Label("MSS_Thirst_Settings_ArchoQuestComponentHuntInterval".Translate());
        options.IntRange(ref ArchoQuestComponentHuntInterval, 60, 864000);

        options.Label("MSS_Thirst_Settings_ArchoQuestSubQuestInterval".Translate());
        options.IntRange(ref ArchoQuestSubQuestInterval, 60, 864000);

        WaterContaminationIncident.WaterItemContaminationCurveReset = true;

        options.Gap();

        options.End();

        Widgets.EndScrollView();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref ThirstCasketHediffTickRate, "ThirstCasketHediffTickRate", 360);
        Scribe_Values.Look(ref MaxSafeWaterInNet, "MaxSafeWaterInNet", 30);
        Scribe_Values.Look(ref SafeWaterPacks, "SafeWaterPacks", 10);
        Scribe_Values.Look(ref MaxUnsafeWaterPacks, "MaxUnsafeWaterPacks", 30);
        Scribe_Values.Look(ref DaysBetweenWaterDestruction, "DaysBetweenWaterDestruction", 10);
        Scribe_Values.Look(ref BadWaterNoticingRollsPerDay, "BadWaterNoticingRollsPerDay", 2);
        Scribe_Values.Look(ref ArchotechComponentsToCompleteBuilding, "ArchotechComponentsToCompleteBuilding", 3);
        Scribe_Values.Look(ref ArchoQuestComponentHuntInterval, "ArchoQuestComponentHuntInterval", new IntRange(3600, 108000));
        Scribe_Values.Look(ref ArchoQuestSubQuestInterval, "ArchoQuestSubQuestInterval", new IntRange(3600, 108000));
    }
}
