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
        public static void SetPawnGradientHair(Pawn pawn, bool enabled, Color color)
        {
            CompGradientHair comp = pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GradientHairSettings settings = comp.Settings;

            settings.enabled = enabled;
            settings.colorB = color;
        }
    }
}
