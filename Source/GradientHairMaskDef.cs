using GradientHair;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
    public class GradientHairMaskDef : StyleItemDef
    {
        public string mask;
        public float weight;

        public string group = "none";
        public int order = 0;

        public GradientHairMaskDef() {
            noGraphic = true;

            styleGender = StyleGender.Any;
        }

        public override Texture2D Icon {
            get
            {
                return ContentFinder<Texture2D>.Get(mask, true);
            }
        }
    }
}
