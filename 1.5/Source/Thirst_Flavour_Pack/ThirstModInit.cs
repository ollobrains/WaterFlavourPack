using HarmonyLib;
using Verse;

namespace Thirst_Flavour_Pack;

[StaticConstructorOnStartup]
public static class ThirstModInit
{
    static ThirstModInit()
    {
        // VE Hard codes the colours rather than using the BarColor getters so we have to tweak them
        if (ModsConfig.IsActive("vanillaracesexpanded.sanguophage"))
        {
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:AnimalHemogenBarTex")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(96, 197, 240).ToColor));
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:AnimalHemogenBarTexHighlight")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(0, 197, 240).ToColor));
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:CorpseHemogenBarTex")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(96, 127, 149).ToColor));
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:CorpseHemogenBarTexHighlight")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(0, 127, 149).ToColor));
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:SanguophageHemogenBarTex")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(96, 147, 159).ToColor));
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:SanguophageHemogenBarTexHighlight")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(0, 147, 159).ToColor));
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:DefaultHemogenBarTex")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(96, 177, 199).ToColor));
            AccessTools.Field("VanillaRacesExpandedSanguophage.GraphicsCache:DefaultHemogenBarTexHighlight")
                .SetValue(null, SolidColorMaterials.NewSolidColorTexture(new ColorInt(0, 177, 199).ToColor));
        }
    }
}
