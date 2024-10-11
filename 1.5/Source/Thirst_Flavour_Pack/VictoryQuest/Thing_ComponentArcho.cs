using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

/// <summary>
/// Lets us use type checks, and overrides CanStackWith to always return false
/// </summary>
public class Thing_ComponentArcho: ThingWithComps
{
    public override bool CanStackWith(Thing other)
    {
        return false;
    }

}
