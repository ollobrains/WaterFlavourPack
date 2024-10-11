using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Thirst_Flavour_Pack.VictoryQuest.MapGen;

public class WorldObjectCompProperties_DefeatAllEnemiesArchospringQuest : WorldObjectCompProperties
{
    public WorldObjectCompProperties_DefeatAllEnemiesArchospringQuest()
    {
        compClass = typeof (DefeatAllEnemiesArchospringQuestComp);
    }

    public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
    {
        foreach (string configError in base.ConfigErrors(parentDef))
            yield return configError;
        if (!typeof (MapParent).IsAssignableFrom(parentDef.worldObjectClass))
            yield return $"{parentDef.defName} has WorldObjectCompProperties_DefeatAllEnemiesArchospringQuest but it's not MapParent.";
    }
}
