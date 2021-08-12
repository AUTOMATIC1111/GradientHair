using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace GradientHair.StylingStation
{

    [HarmonyPatch(typeof(Dialog_StylingStation), MethodType.Constructor, new Type[] { typeof(Pawn), typeof(Thing) })]
    class PatchDialog_StylingStation
    {
        static void Prefix(Pawn pawn) {
            GlobalStylingStation.hairGradientScrollPosition = default;

            CompGradientHair comp = pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GlobalStylingStation.initialSettings.enabled = comp.Settings.enabled;
            GlobalStylingStation.initialSettings.mask = comp.Settings.mask;
            GlobalStylingStation.initialSettings.colorB = comp.Settings.colorB;
        }
    }

    [HarmonyPatch(typeof(Dialog_StylingStation), "DoWindowContents")]
    class PatchDialog_StylingStationDoWindowContents
    {
        static void Prefix(Dialog_StylingStation __instance, Pawn ___pawn)
        {
            GlobalStylingStation.dialog = __instance;
        }
        static void Postfix()
        {
            GlobalStylingStation.dialog = null;
        }
    }


    [HarmonyPatch(typeof(Dialog_StylingStation), "DrawTabs")]
    class PatchDialog_StylingStationDrawTabs
    {
        static void Postfix(Dialog_StylingStation __instance, Rect rect, Pawn ___pawn, Color ___desiredHairColor)
        {
            if(Traverse.Create(__instance).Field("curTab").GetValue<int>() != GlobalStylingStation.tabIndex) return;
            CompGradientHair comp = ___pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            rect.yMax -= 110f;

            Action<Rect, GradientHairMaskDef> drawAction = delegate (Rect r, GradientHairMaskDef h)
            {
                Graphic_MultiMask graphic = GlobalStylingStation.GraphicForHair(___pawn.story.hairDef, h);
                graphic.SetColors(___desiredHairColor, comp.Settings.colorB);
                graphic.Draw(r.ExpandedBy(r.width * 0.125f));

                if(h.modContentPack != GradientHair.myOwnPack)
                {
                    TooltipHandler.TipRegion(r, new TipSignal("GradientHairAddedByMod".Translate(h.modContentPack.Name)));
                }
                
            };
            Action<GradientHairMaskDef> selectAction = delegate (GradientHairMaskDef h)
            {
                comp.Settings.mask = h.mask;
                comp.Settings.enabled = h.defName != "MaskNone";
            };
            Func<StyleItemDef, bool> hasStyleItem = x =>
            {
                GradientHairMaskDef h = x as GradientHairMaskDef;
                if (!comp.Settings.enabled) return h.defName == "MaskNone";

                return comp.Settings.mask == h.mask;
            };
            Func<StyleItemDef, bool> hadStyleItem = x => true;

            MethodInfo methodA = typeof(Dialog_StylingStation).GetMethod("DrawStylingItemType", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo method = methodA.MakeGenericMethod(typeof(GradientHairMaskDef));
            method.Invoke(__instance, new object[] { rect, GlobalStylingStation.hairGradientScrollPosition, drawAction, selectAction, hasStyleItem, hadStyleItem, null, false });

            GlobalStylingStation.DrawHairColors(new Rect(rect.x, rect.yMax + 10f, rect.width, 110f), comp.Settings);
        }
    }

    [HarmonyPatch(typeof(Dialog_StylingStation), "Reset")]
    class PatchDialog_StylingStationReset
    {
        static void Prefix(Pawn ___pawn)
        {
            CompGradientHair comp = ___pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            comp.Settings.enabled = GlobalStylingStation.initialSettings.enabled;
            comp.Settings.mask = GlobalStylingStation.initialSettings.mask;
            comp.Settings.colorB = GlobalStylingStation.initialSettings.colorB;
        }
    }

    // if we modified gradient hair settings, force an invisible change to main hair color so that the accept button does something
    [HarmonyPatch(typeof(Dialog_StylingStation), "DrawBottomButtons")]
    class PatchDialog_StylingStationDrawBottomButtons
    {
        static void Prefix(Pawn ___pawn, ref Color ___desiredHairColor)
        {
            GlobalStylingStation.tmpDesiredHairColor = ___desiredHairColor;

            CompGradientHair comp = ___pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            if (!comp.Settings.SameAs(GlobalStylingStation.initialSettings))
            {
                ___desiredHairColor.a += 0.0001f;
            }
        }

        static void Postfix(Pawn ___pawn, ref Color ___desiredHairColor)
        {
            CompGradientHair comp = ___pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            ___desiredHairColor = GlobalStylingStation.tmpDesiredHairColor;
        }
    }

    

}
