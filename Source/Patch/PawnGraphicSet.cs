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
    [HarmonyPatch(typeof(PawnRenderNode_Hair), "GraphicFor")]
    class PatchPawnRenderNode_HairGraphicFor
    {
        static void Postfix(Pawn pawn, ref Graphic __result)
        {
            CompGradientHair comp = pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GradientHairSettings settings = comp.Settings;
            if (!settings.enabled) return;

            __result = Graphic_MultiMask.Get(pawn.story.hairDef.texPath, settings.mask, pawn.story.HairColor, settings.colorB);
        }
    }
}
