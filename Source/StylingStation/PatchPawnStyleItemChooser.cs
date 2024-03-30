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
    [HarmonyPatch(typeof(PawnStyleItemChooser), "StyleItemChoiceLikelihoodFromTags")]
    class PatchPawnStyleItemChooserStyleItemChoiceLikelihoodFromTags
    {
        static bool Prefix(ref float __result, StyleItemDef styleItem)
        {
            if (!(styleItem is GradientHairMaskDef)) return true;

            __result = -styleItem.index;

            return false;
        }
    }

    // prevent the bug in HAR related to missing key dict in https://github.com/erdelf/AlienRaces/blob/c854ad7845801ac9d98bcf6b09e9af7ec9e36742/Source/AlienRace/AlienRace/HarmonyPatches.cs#L525
    [HarmonyPatch(typeof(PawnStyleItemChooser), "WantsToUseStyle")]
    class PatchPawnStyleItemChooserWantsToUseStyle
    {
        [HarmonyBefore(new string[] { "rimworld.erdelf.alien_race.main" })]
        static bool Prefix(ref bool __result, Pawn pawn, StyleItemDef styleItemDef)
        {
            if (styleItemDef is GradientHairMaskDef)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
