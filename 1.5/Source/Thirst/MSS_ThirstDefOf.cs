using RimWorld;
using Verse;

namespace MSS_Thirst;

[DefOf]
public static class MSS_ThirstDefOf
{
    // Remember to annotate any Defs that require a DLC as needed e.g.
    // [MayRequireBiotech]
    // public static GeneDef YourPrefix_YourGeneDefName;

    static MSS_ThirstDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(MSS_ThirstDefOf));
}
