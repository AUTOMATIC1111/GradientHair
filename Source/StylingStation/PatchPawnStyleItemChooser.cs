using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GradientHair.StylingStation
{

    // the result of this function is used for ordering in the dialog
    [HarmonyPatch(typeof(PawnStyleItemChooser), "StyleItemChoiceLikelihoodFor")]
    class PatchPawnStyleItemChooser
    {
        static bool Prefix(ref float __result, StyleItemDef styleItem)
        {
            if (!(styleItem is GradientHairMaskDef)) return true;

            __result = -styleItem.index;

            return false;
        }
    }
}
