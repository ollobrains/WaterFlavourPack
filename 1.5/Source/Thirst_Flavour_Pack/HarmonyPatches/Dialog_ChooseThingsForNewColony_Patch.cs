using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Thirst_Flavour_Pack.VictoryQuest;
using UnityEngine;

namespace Thirst_Flavour_Pack.HarmonyPatches;


[HarmonyPatch(typeof(Dialog_ChooseThingsForNewColony))]
[HarmonyPatch(nameof(Dialog_ChooseThingsForNewColony.DoWindowContents))]
public static class Dialog_ChooseThingsForNewColony_Patch
{
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
        foreach (CodeInstruction instruction in codes)
        {
            if (instruction.opcode == OpCodes.Ldstr)
            {
                if ((string) instruction.operand == "ChooseThingsForNewColonyTitle")
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(
                        OpCodes.Call,
                        AccessTools.Method(typeof(Dialog_ChooseThingsForNewColony_Patch), nameof(GetTranslatedTitle))
                    );
                }else if ((string) instruction.operand == "ChooseThingsForNewColonyDesc")
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(
                        OpCodes.Call,
                        AccessTools.Method(typeof(Dialog_ChooseThingsForNewColony_Patch), nameof(GetTranslatedDesc))
                    );
                }
                else
                {
                    yield return instruction;
                }
            }else if (instruction.opcode == OpCodes.Ldc_R4 && Mathf.Approximately((float) instruction.operand, 25f))
            {
                // this should hit 3 times for the 3 instances of "25f"
                yield return new CodeInstruction(OpCodes.Ldc_R4, 100f);
            }
            else
            {
                yield return instruction;
            }
        }
    }

    private static string GetTranslatedTitle(Dialog_ChooseThingsForNewColony instance)
    {
        return instance is Dialog_ChooseThingsForNewColony_ArchospringQuest ? "MSS_Thirst_ChooseThingsForNewColonyTitle"
            : "ChooseThingsForNewColonyTitle";
    }

    private static string GetTranslatedDesc(Dialog_ChooseThingsForNewColony instance)
    {
        return instance is Dialog_ChooseThingsForNewColony_ArchospringQuest ? "MSS_Thirst_ChooseThingsForNewColonyDesc"
            : "ChooseThingsForNewColonyDesc";
    }
}
