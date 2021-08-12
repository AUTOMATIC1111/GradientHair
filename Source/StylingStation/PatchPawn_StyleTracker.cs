using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace GradientHair.StylingStation
{
    [HarmonyPatch(typeof(Pawn_StyleTracker), "SetupNextLookChangeData")]
    class PatchPawn_StyleTrackerSetupNextLookChangeData
    {
        static void Prefix(Pawn_StyleTracker __instance)
        {
            CompGradientHair comp = __instance.pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            if (!comp.Settings.SameAs(GlobalStylingStation.initialSettings))
            {
                comp.Settings.desiredEnabled = comp.Settings.enabled;
                comp.Settings.desiredMask = comp.Settings.mask;
                comp.Settings.desiredColorB = comp.Settings.colorB;
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_StyleTracker), "FinalizeHairColor")]
    class PatchPawn_StyleTrackerFinalizeHairColor
    {
        static void Prefix(Pawn_StyleTracker __instance)
        {

            CompGradientHair comp = __instance.pawn.GetComp<CompGradientHair>();
            if (comp == null) return;
            Log.Message("FinalizeHairColor called");

            if (comp.Settings.desiredEnabled != null)
            {
                comp.Settings.enabled = comp.Settings.desiredEnabled.Value;
                comp.Settings.desiredEnabled = null;
            }
            if (comp.Settings.desiredMask != null)
            {
                comp.Settings.mask = comp.Settings.desiredMask;
                comp.Settings.desiredMask = null;
            }
            if (comp.Settings.desiredColorB != null)
            {
                comp.Settings.colorB = comp.Settings.desiredColorB.Value;
                comp.Settings.desiredColorB = null;
            }
        }
    }

}
