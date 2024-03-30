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

        public override Graphic GraphicFor(Pawn pawn, Color color)
        {

            if (noGraphic)
            {
                return null;
            }

            string texPath = this.texPath;
            ShaderTypeDef overrideShaderTypeDef = this.overrideShaderTypeDef;
            return GraphicDatabase.Get<Graphic_Multi>(texPath, ((overrideShaderTypeDef != null) ? overrideShaderTypeDef.Shader : null) ?? ShaderDatabase.Cutout, Vector2.one, color);
        }
    }
}
