using GradientHair.StylingStation;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair.Patch
{
    [HarmonyPatch(typeof(PawnGraphicSet), "CalculateHairMats")]
    class PawnGraphicSetCalculateHairMats
    {
        static void Postfix(PawnGraphicSet __instance)
        {
            CompGradientHair comp = __instance.pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GradientHairSettings settings = comp.Settings;
            if (!settings.enabled) return;

            __instance.hairGraphic = Graphic_MultiMask.Get(__instance.pawn.story.hairDef.texPath, settings.mask, __instance.pawn.story.hairColor, settings.colorB);
        }
    }
}
