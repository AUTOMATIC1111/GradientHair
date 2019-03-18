using EdB.PrepareCarefully;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void Postfix(CustomPawn customPawn) {
            Pawn pawn = customPawn.Pawn;
            CompGradientHair comp = pawn.GetComp<CompGradientHair>();
            if (comp == null) return;

            GradientHairSettings settings = comp.Settings;

            Rect rectToggle = settings.enabled ? rectToggleSmall : rectToggleFull;
            TooltipHandler.TipRegion(rectToggle, "GradientHairEnable".Translate());
            bool enable = GUI.Toggle(rectToggle, settings.enabled,settings.enabled ? "" : "GradientHairEnable".Translate());
            if (enable != comp.Settings.enabled)
            {
                settings.enabled = enable;
                if(enable) settings.RandomizeTexture();
                customPawn.MarkPortraitAsDirty();
            }

            if (!enable) return;

            settings.colorA = pawn.story.hairColor;

            Rect rect = new Rect(40, 381, 27, 27);
            Color color = settings.colorB;

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
                settings.colorB = new Color(vr, vg, vb);
                customPawn.MarkPortraitAsDirty();
            }

            GUI.color = Color.white;
        }
    }
}
