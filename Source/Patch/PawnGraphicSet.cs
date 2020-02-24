using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair.Patch
{
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveApparelGraphics")]
    class PawnGraphicSetResolveApparelGraphics
    {
        static void Prefix(PawnGraphicSet __instance)
        {
            CompGradientHair comp = __instance.pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GradientHairSettings settings = comp.Settings;
            if (!settings.enabled) return;

            string path = __instance.pawn.story.hairDef.texPath + "\0" + settings.mask;
            __instance.hairGraphic = GraphicDatabase.Get<Graphic_MultiMask>(path, ShaderDatabase.CutoutComplex, Vector2.one, __instance.pawn.story.hairColor, settings.colorB);
        }
    }

}
