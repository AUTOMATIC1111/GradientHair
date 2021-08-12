using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace GradientHair.StylingStation
{
    // add a new "Gradient" tab to the list of tabs drawn for the dialog
    [HarmonyPatch]
    class PatchTabDrawer
    {
        static MethodInfo TargetMethod()
        {
            return typeof(TabDrawer)
                .GetMethods()
                .Single(m => m.Name == "DrawTabs" && m.IsGenericMethodDefinition)
                .MakeGenericMethod(typeof(TabRecord));

        }

        static void Prefix(List<TabRecord> tabs) {
            if (GlobalStylingStation.dialog == null) return;

            string searchedTitle = "Hair".Translate().CapitalizeFirst();
            int index = tabs.FirstIndexOf(x => x.label == searchedTitle);
            if (index == -1) return;

            var curTab = Traverse.Create(GlobalStylingStation.dialog).Field("curTab");

            tabs.Insert(index+1, new TabRecord("GradientHairTab".Translate().CapitalizeFirst(), delegate ()
            {
                curTab.SetValue(GlobalStylingStation.tabIndex);
            }, curTab.GetValue<int>() == GlobalStylingStation.tabIndex));
        }
    }
}
