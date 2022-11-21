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
    public class GlobalStylingStation
    {
        public const int tabIndex = 993;
        public static GradientHairSettings initialSettings = new GradientHairSettings();
        public static Dialog_StylingStation dialog = null;
        public static Vector2 hairGradientScrollPosition = default;
        public static Color tmpDesiredHairColor;


        public static Graphic_MultiMask GraphicForHair(HairDef hair, GradientHairMaskDef gradient)
        {
            return Graphic_MultiMask.Get(hair.texPath, gradient.mask, new Color(1, 1, 1, 1.993f), new Color(1, 1, 1, 1.993f)) as Graphic_MultiMask;
        }

        public static void DrawHairColors(Rect rect, GradientHairSettings settings)
        {
            if (dialog == null) return;
            float num = rect.y;
            Widgets.ColorSelector(new Rect(rect.x, num, rect.width, 92f), ref settings.colorB,  AllHairColors, out _, null, 22, 2);
            //num += 60f;
        }

        private static List<Color> allHairColors;
        

        private static List<Color> AllHairColors
        {
            get
            {
                if (allHairColors == null)
                {
                    allHairColors = (from ic in DefDatabase<ColorDef>.AllDefsListForReading select ic.color).ToList<Color>();
                    allHairColors.SortByColor((Color x) => x);
                }
                return allHairColors;
            }
        }
    }
}
