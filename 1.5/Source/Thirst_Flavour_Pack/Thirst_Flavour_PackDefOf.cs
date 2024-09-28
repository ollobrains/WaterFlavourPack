using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack;

[DefOf]
public static class Thirst_Flavour_PackDefOf
{
    // Remember to annotate any Defs that require a DLC as needed e.g.
    // [MayRequireBiotech]
    // public static GeneDef YourPrefix_YourGeneDefName;

    public static HediffDef MSSThirst_Extracted_Water;

    static Thirst_Flavour_PackDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(Thirst_Flavour_PackDefOf));
}
