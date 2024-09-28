using UnityEngine;
using Verse;

namespace Thirst_Flavour_Pack;

public class Settings : ModSettings
{
    public int ThirstCasketHediffTickRate = 360;

    public void DoWindowContents(Rect wrect)
    {
        Listing_Standard options = new Listing_Standard();
        options.Begin(wrect);
        options.Label("MSS_Thirst_ThirstCasketHediffTickRate".Translate(ThirstCasketHediffTickRate));
        options.IntAdjuster(ref ThirstCasketHediffTickRate, 1, 0);
        options.Gap();

        options.End();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref ThirstCasketHediffTickRate, "ThirstCasketHediffTickRate", 360);
    }
}
