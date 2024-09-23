﻿using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class QuestPart_RequirementToAcceptPowerRegulatorBuilt: QuestPart_RequirementsToAccept
{
    public override AcceptanceReport CanAccept()
    {
        return WaterVictoryWorldComponent.Instance.PowerRegulatorsBuilt < 3 ? new AcceptanceReport("MSS_Thirst_QuestPowerRegRequired".Translate(WaterVictoryWorldComponent.Instance.PowerRegulatorsBuilt, 3)) : true;
    }
}
