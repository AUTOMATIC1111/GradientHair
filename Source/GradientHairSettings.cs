using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair
{

    public class GradientHairSettings : IExposable
    {
        public GradientHairSettings() {

        }

        static System.Random random = new System.Random();

        public bool enabled = false;
        public Color colorB;
        public string mask;

        public bool? desiredEnabled;
        public Color? desiredColorB;
        public string desiredMask;

        public bool SameAs(GradientHairSettings other) {
            if (!enabled && !other.enabled) return true;

            return enabled == other.enabled && colorB == other.colorB && mask == other.mask;
        }

        public void Randomize(Pawn pawn)
        {
            if (pawn == null) return;

            if (GradientHair.settings.enable)
            {
                float chance = 0f;

                if (pawn.gender == Gender.Male) chance = GradientHair.settings.chanceMale;
                if (pawn.gender == Gender.Female) chance = GradientHair.settings.chanceFemale;

                enabled = random.NextDouble() < chance;
            }
            else
            {
                enabled = false;
            }

            RandomizeTexture();

            Color colorA;
            RandomizeColors(out colorA);

            if (enabled)
            {
                pawn.story.HairColor = colorA;
            }
        }

        public void RandomizeTexture()
        {
            GradientHairMaskDef def = DefDatabase<GradientHairMaskDef>.AllDefs.RandomElementByWeight(x => x.weight);
            mask = def.mask;
        }

        public void RandomizeColors(out Color colorA)
        {
            colorA = Color.white;

            for (int i = 0; i < 10; i++)
            {
                float hueA = (float)random.NextDouble();
                float hueB = (float)random.NextDouble();
                float hueDiff = Math.Abs(hueA - hueB);
                if (hueDiff > 0.5f) hueDiff = 0.5f - hueDiff;
                if (hueDiff < 0.15f) continue;

                colorA = Color.HSVToRGB(hueA, 0.5f + (float)random.NextDouble() * 0.5f, 0.5f + (float)random.NextDouble() * 0.5f);
                colorB = Color.HSVToRGB(hueB, 0.5f + (float)random.NextDouble() * 0.5f, 0.5f + (float)random.NextDouble() * 0.5f);

                float distance = (Math.Abs(colorA.r - colorB.r) + Math.Abs(colorA.g - colorB.g) + Math.Abs(colorA.b - colorB.b)) / 3;
                if (distance < 0.15f) continue;

                return;
            }
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref enabled, "enabled");
            Scribe_Values.Look(ref colorB, "colorB");
            Scribe_Values.Look(ref mask, "mask");
            
            Scribe_Values.Look(ref desiredEnabled, "desiredEnabled");
            Scribe_Values.Look(ref desiredColorB, "desiredColorB");
            Scribe_Values.Look(ref desiredMask, "desiredMask");
        }
    }
}
