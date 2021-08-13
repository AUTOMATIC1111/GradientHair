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
    // this actually changes the hair graphic to a gradient-enabled one
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

    // we do it again in case another mod overweites it
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveApparelGraphics")]
    class PawnGraphicSetResolveApparelGraphics
    {
        static void Postfix(PawnGraphicSet __instance)
        {
            CompGradientHair comp = __instance.pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GradientHairSettings settings = comp.Settings;
            if (!settings.enabled) return;

            if (__instance.hairGraphic is Graphic_MultiMask) return;
            __instance.hairGraphic = Graphic_MultiMask.Get(__instance.pawn.story.hairDef.texPath, settings.mask, __instance.pawn.story.hairColor, settings.colorB);
        }
    }
}
