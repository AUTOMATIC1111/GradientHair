﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair.Patch
{
    class PanelAppearanceDrawColorSelectorForPawnLayer
    {
        protected static bool CloseEnough(float a, float b)
        {
            return a > b - 0.0001f && a < b + 0.0001f;
        }

        static Rect rectToggleSmall = new Rect(19, 383, 17, 23);
        static Rect rectToggleFull = new Rect(19, 383, 192, 23);

        public static void Postfix(object customPawn) {
            var trv = Traverse.Create(customPawn);
            Pawn pawn = trv.Field<Pawn>("pawn").Value;

            bool enabled;
            Color color;
            if (!PublicApi.GetGradientHair(pawn, out enabled, out color))
                return;
            
            Rect rectToggle = enabled ? rectToggleSmall : rectToggleFull;
            TooltipHandler.TipRegion(rectToggle, "GradientHairEnable".Translate());
            bool enable = GUI.Toggle(rectToggle, enabled, enabled ? "".Translate() : "GradientHairEnable".Translate());
            if (enable != enabled)
            {
                PublicApi.SetGradientHair(pawn, enable, color);
                trv.Method("MarkPortraitAsDirty").GetValue();
            }

            if (!enable) return;

            Rect rect = new Rect(40, 381, 27, 27);

            GUI.color = Color.white;
            GUI.DrawTexture(rect, BaseContent.WhiteTex);
            GUI.color = color;
            GUI.DrawTexture(GenUI.ContractedBy(rect, 1f), BaseContent.WhiteTex);

            GUI.color = Color.red;
            float r = color.r;
            float vr = GUI.HorizontalSlider(new Rect(rect.x + 34f, rect.y - 3f, 136f, 11f), color.r, 0f, 1f);

            GUI.color = Color.green;
            float g = color.g;
            float vg = GUI.HorizontalSlider(new Rect(rect.x + 34f, rect.y + 8f, 136f, 11f), color.g, 0f, 1f);

            GUI.color = Color.blue;
            float b = color.b;
            float vb = GUI.HorizontalSlider(new Rect(rect.x + 34f, rect.y + 19f, 136f, 11f), color.b, 0f, 1f);

            if (!CloseEnough(vr, r) || !CloseEnough(vg, g) || !CloseEnough(vb, b))
            {
                PublicApi.SetGradientHair(pawn, true, new Color(vr, vg, vb));
                trv.Method("MarkPortraitAsDirty").GetValue();
            }

            GUI.color = Color.white;
        }
    }
}
