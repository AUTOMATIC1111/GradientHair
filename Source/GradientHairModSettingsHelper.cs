using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace GradientHair
{
    public static class GradientHairModSettingsHelper
    {

        public static void SliderLabeled(this Listing_Standard listing, string label, ref float val, string tooltip, float min, float max, string format)
        {
            Rect rect = listing.GetRect(Text.LineHeight);

            TextAnchor anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect.LeftPart(0.5f), label);
            val = Widgets.HorizontalSlider(rect.RightPart(0.5f).LeftPart(0.8f), val, min, max, true);
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.Label(rect.RightPart(0.1f), string.Format(format, val));
            if (!tooltip.NullOrEmpty()) TooltipHandler.TipRegion(rect, tooltip);
            Text.Anchor = anchor;

            listing.Gap(listing.verticalSpacing);
        }
    }
}
