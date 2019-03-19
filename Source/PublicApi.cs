using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair
{
    public class PublicApi
    {
        public static bool GetGradientHair(Pawn pawn, out bool enabled, out Color color)
        {
            CompGradientHair comp = pawn.GetComp<CompGradientHair>();
            if (comp == null)
            {
                enabled = false;
                color = Color.white;
                return false;
            }

            GradientHairSettings settings = comp.Settings;
            enabled = settings.enabled;
            color = settings.colorB;
            return true;
        }

        public static void SetGradientHair(Pawn pawn, bool enabled, Color color)
        {
            CompGradientHair comp = pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GradientHairSettings settings = comp.Settings;

            settings.enabled = enabled;
            settings.colorB = color;
        }
    }
}
