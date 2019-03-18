using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace GradientHair
{
    public class CompGradientHair : ThingComp
    {
        public CompGradientHair() {
        }

        private GradientHairSettings settings;

        public GradientHairSettings Settings {
            get {
                if (settings == null)
                {
                    settings = new GradientHairSettings();
                    settings.Randomize(parent as Pawn);
                }

                return settings;
            }
        }

        public CompPropertiesGradientHair Props => (CompPropertiesGradientHair)props;

        public override void PostExposeData() {
            Scribe_Deep.Look(ref settings, "gradientHair", new object[0]);
        }
    }
}
