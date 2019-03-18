using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair
{
    class Graphic_MultiMask : Graphic
    {
        // Token: 0x17000D2F RID: 3375
        // (get) Token: 0x0600520A RID: 21002 RVA: 0x00260520 File Offset: 0x0025E920
        public string GraphicPath
        {
            get
            {
                return path;
            }
        }

        // Token: 0x17000D30 RID: 3376
        // (get) Token: 0x0600520B RID: 21003 RVA: 0x00260528 File Offset: 0x0025E928
        public override Material MatSingle
        {
            get
            {
                return MatSouth;
            }
        }

        // Token: 0x17000D31 RID: 3377
        // (get) Token: 0x0600520C RID: 21004 RVA: 0x00260530 File Offset: 0x0025E930
        public override Material MatWest
        {
            get
            {
                return mats[3];
            }
        }

        // Token: 0x17000D32 RID: 3378
        // (get) Token: 0x0600520D RID: 21005 RVA: 0x0026053A File Offset: 0x0025E93A
        public override Material MatSouth
        {
            get
            {
                return mats[2];
            }
        }

        // Token: 0x17000D33 RID: 3379
        // (get) Token: 0x0600520E RID: 21006 RVA: 0x00260544 File Offset: 0x0025E944
        public override Material MatEast
        {
            get
            {
                return mats[1];
            }
        }

        // Token: 0x17000D34 RID: 3380
        // (get) Token: 0x0600520F RID: 21007 RVA: 0x0026054E File Offset: 0x0025E94E
        public override Material MatNorth
        {
            get
            {
                return mats[0];
            }
        }

        // Token: 0x17000D35 RID: 3381
        // (get) Token: 0x06005210 RID: 21008 RVA: 0x00260558 File Offset: 0x0025E958
        public override bool WestFlipped
        {
            get
            {
                return westFlipped;
            }
        }

        // Token: 0x17000D36 RID: 3382
        // (get) Token: 0x06005211 RID: 21009 RVA: 0x00260560 File Offset: 0x0025E960
        public override bool EastFlipped
        {
            get
            {
                return eastFlipped;
            }
        }

        // Token: 0x17000D37 RID: 3383
        // (get) Token: 0x06005212 RID: 21010 RVA: 0x00260568 File Offset: 0x0025E968
        public override bool ShouldDrawRotated
        {
            get
            {
                return (data == null || data.drawRotated) && (MatEast == MatNorth || MatWest == MatNorth);
            }
        }

        // Token: 0x17000D38 RID: 3384
        // (get) Token: 0x06005213 RID: 21011 RVA: 0x002605BC File Offset: 0x0025E9BC
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
            string mask = args[1];
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
                        Log.Error("Failed to find any texture while constructing " + GraphicPath + ". Filenames have changed; if you are converting an old mod, recommend renaming textures from *_back to *_north, *_side to *_east, and *_front to *_south.", false);
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
                array2[0] = ContentFinder<Texture2D>.Get(mask, false);
                array2[1] = ContentFinder<Texture2D>.Get(mask, false);
                array2[2] = ContentFinder<Texture2D>.Get(mask, false);
                array2[3] = ContentFinder<Texture2D>.Get(mask, false);
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


        // Token: 0x06005215 RID: 21013 RVA: 0x00260966 File Offset: 0x0025ED66
        public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
        {
            return GraphicDatabase.Get<Graphic_Multi>(path, newShader, drawSize, newColor, newColorTwo, data);
        }

        // Token: 0x06005216 RID: 21014 RVA: 0x00260984 File Offset: 0x0025ED84
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

        // Token: 0x06005217 RID: 21015 RVA: 0x002609E4 File Offset: 0x0025EDE4
        public override int GetHashCode()
        {
            int seed = 0;
            seed = Gen.HashCombine<string>(seed, path);
            seed = Gen.HashCombineStruct<Color>(seed, color);
            return Gen.HashCombineStruct<Color>(seed, colorTwo);
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
