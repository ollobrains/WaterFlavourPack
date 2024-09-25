using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace Thirst_Flavour_Pack.VictoryQuest;

[StaticConstructorOnStartup]
public class Reward_WaterMap : Reward
{
    public int currentPart = 1;
    private const int totalParts = 3;
    private static Texture2D icon;

    private static Texture2D Icon
    {
        get
        {
            if (icon == null)
                icon = ContentFinder<Texture2D>.Get("UI/Icons/ArchonexusMapPart");
            return icon;
        }
    }

    public override IEnumerable<GenUI.AnonymousStackElement> StackElements
    {
        get
        {
            yield return QuestPartUtility.GetStandardRewardStackElement(
                "MSS_Reward_WaterMapPartLabel".Translate(currentPart, 3), Icon, () => GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
        }
    }

    public override void InitFromValue(
        float rewardValue,
        RewardsGeneratorParams parms,
        out float valueActuallyUsed)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<QuestPart> GenerateQuestParts(
        int index,
        RewardsGeneratorParams parms,
        string customLetterLabel,
        string customLetterText,
        RulePack customLetterLabelRules,
        RulePack customLetterTextRules)
    {
        throw new NotImplementedException();
    }

    public override string GetDescription(RewardsGeneratorParams parms)
    {
        return "MSS_Reward_WaterMapPart".Translate(currentPart, 3);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref currentPart, "currentPart");
    }
}
