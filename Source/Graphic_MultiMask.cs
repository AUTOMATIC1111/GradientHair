using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair
{
    public class Graphic_MultiMask : Graphic
    {
        public static Graphic_MultiMask Get(string hairTexture, string maskTexture, Color hairColor, Color gradientColor) {
            return GraphicDatabase.Get<Graphic_MultiMask>(hairTexture + "\0" + maskTexture, ShaderDatabase.CutoutComplex, Vector2.one, hairColor, gradientColor) as Graphic_MultiMask;
        }

        public string GraphicPath
        {
            get
            {
                return path;
            }
        }

        public override Material MatSingle
        {
            get
            {
                return MatSouth;
            }
        }

        public override Material MatWest
        {
            get
            {
                return mats[3];
            }
        }

        public override Material MatSouth
        {
            get
            {
                return mats[2];
            }
        }

        public override Material MatEast
        {
            get
            {
                return mats[1];
            }
        }

        public override Material MatNorth
        {
            get
            {
                return mats[0];
            }
        }

        public override bool WestFlipped
        {
            get
            {
                return westFlipped;
            }
        }

        public override bool EastFlipped
        {
            get
            {
                return eastFlipped;
            }
        }

        public override bool ShouldDrawRotated
        {
            get
            {
                return (data == null || data.drawRotated) && (MatEast == MatNorth || MatWest == MatNorth);
            }
        }

        public override float DrawRotatedExtraAngleOffset
        {
            get
            {
                return drawRotatedExtraAngleOffset;
            }
        }

        public override void Init(GraphicRequest req)
        {
            string[] args = req.path.Split('\0');
            maskPath = args.Length > 1 ? args[1] : req.maskPath;
            path = args[0];
            data = req.graphicData;
            color = req.color;
            colorTwo = req.colorTwo;
            drawSize = req.drawSize;
            Texture2D[] array = new Texture2D[mats.Length];
            array[0] = ContentFinder<Texture2D>.Get(path + "_north", false);
            array[1] = ContentFinder<Texture2D>.Get(path + "_east", false);
            array[2] = ContentFinder<Texture2D>.Get(path + "_south", false);
            array[3] = ContentFinder<Texture2D>.Get(path + "_west", false);
            if (array[0] == null)
            {
                if (array[2] != null)
                {
                    array[0] = array[2];
                    drawRotatedExtraAngleOffset = 180f;
                }
                else if (array[1] != null)
                {
                    array[0] = array[1];
                    drawRotatedExtraAngleOffset = -90f;
                }
                else
                {
                    if (!(array[3] != null))
                    {
                        Log.Error("Failed to find any texture while constructing " + GraphicPath + ". Filenames have changed; if you are converting an old mod, recommend renaming textures from *_back to *_north, *_side to *_east, and *_front to *_south.");
                        return;
                    }
                    array[0] = array[3];
                    drawRotatedExtraAngleOffset = 90f;
                }
            }
            if (array[2] == null)
            {
                array[2] = array[0];
            }
            if (array[1] == null)
            {
                if (array[3] != null)
                {
                    array[1] = array[3];
                    eastFlipped = base.DataAllowsFlip;
                }
                else
                {
                    array[1] = array[0];
                }
            }
            if (array[3] == null)
            {
                if (array[1] != null)
                {
                    array[3] = array[1];
                    westFlipped = base.DataAllowsFlip;
                }
                else
                {
                    array[3] = array[0];
                }
            }
            Texture2D[] array2 = new Texture2D[mats.Length];
            if (req.shader.SupportsMaskTex())
            {
                array2[0] = ContentFinder<Texture2D>.Get(maskPath, false);
                array2[1] = ContentFinder<Texture2D>.Get(maskPath, false);
                array2[2] = ContentFinder<Texture2D>.Get(maskPath, false);
                array2[3] = ContentFinder<Texture2D>.Get(maskPath, false);
            }
            for (int i = 0; i < mats.Length; i++)
            {
                MaterialRequest req2 = default(MaterialRequest);
                req2.mainTex = array[i];
                req2.shader = req.shader;
                req2.color = color;
                req2.colorTwo = colorTwo;
                req2.maskTex = array2[i];
                req2.shaderParameters = req.shaderParameters;
                mats[i] = MaterialPool.MatFrom(req2);
            }
        }


        public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
        {
            return GraphicDatabase.Get<Graphic_Multi>(path, newShader, drawSize, newColor, newColorTwo, data);
        }

        public override string ToString()
        {
            return string.Concat(new object[]
            {
                "MultiMask(initPath=",
                path,
                ", color=",
                color,
                ", colorTwo=",
                colorTwo,
                ")"
            });
        }

        public override int GetHashCode()
        {
            int seed = 0;
            seed = Gen.HashCombine<string>(seed, path);
            seed = Gen.HashCombineStruct<Color>(seed, color);
            return Gen.HashCombineStruct<Color>(seed, colorTwo);
        }

        public void SetColors(Color newColor, Color newColorTwo) {

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].color = newColor;
                mats[i].SetColor(ShaderPropertyIDs.ColorTwo, newColorTwo);
            }
        }

        static Rect rect0011 = new Rect(0f, 0f, 1f, 1f);
        public void Draw(Rect rect)
        {
            Material material = MatSouth;
            Texture texture = material.mainTexture;

            if (texture.width != texture.height && rect.width == rect.height)
            {
                float ratio = texture.width / texture.height;
                if (ratio < 1) { rect.x += (rect.width - rect.width * ratio) / 2; rect.width *= ratio; }
                else { rect.y += (rect.height - rect.height / ratio) / 2; rect.height /= ratio; };
            }

            Graphics.DrawTexture(rect, texture, rect0011, 0, 0, 0, 0, new Color(1, 1, 1, GUI.color.a), material, -1);
        }

        // Token: 0x040036A4 RID: 13988
        private Material[] mats = new Material[4];

        // Token: 0x040036A5 RID: 13989
        private bool westFlipped;

        // Token: 0x040036A6 RID: 13990
        private bool eastFlipped;

        // Token: 0x040036A7 RID: 13991
        private float drawRotatedExtraAngleOffset;
    }
}
