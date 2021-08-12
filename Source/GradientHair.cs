using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using Verse;
using GradientHair.Patch;

namespace GradientHair
{
    public class GradientHair : Mod
    {
        public static GradientHairModSettings settings;
        public static ModContentPack myOwnPack;

        public GradientHair(ModContentPack pack) : base(pack)
        {
            var harmony = new Harmony("com.github.automatic1111.gradienthair");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Type panelAppearance = GenTypes.GetTypeInAnyAssembly("EdB.PrepareCarefully.PanelAppearance");
            try
            {
                if (panelAppearance != null)
                    harmony.Patch(AccessTools.Method(panelAppearance, "DrawColorSelectorForPawnLayer"), null, new HarmonyMethod(typeof(PanelAppearanceDrawColorSelectorForPawnLayer), "Postfix"));
            }
            catch (Exception e)
            {
                Log.Error("Failed to patch EdB.PrepareCarefully.PanelAppearance: " + e.ToString());
            }

            settings = GetSettings<GradientHairModSettings>();
            myOwnPack = pack;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("SettingEnableRandomName".Translate(), ref settings.enable, "SettingEnableRandomDesc".Translate());
            listing_Standard.SliderLabeled("SettingChanceMaleName".Translate(), ref settings.chanceMale, "SettingChanceMaleDesc".Translate(), 0, 1, settings.chanceMale.ToStringPercent());
            listing_Standard.SliderLabeled("SettingChanceFemaleName".Translate(), ref settings.chanceFemale, "SettingChanceFemaleDesc".Translate(), 0, 1, settings.chanceFemale.ToStringPercent());
            listing_Standard.End();
        }

        public override string SettingsCategory()
        {
            return "GradientHairTitle".Translate();
        }

    }
}
