using Verse;

namespace GradientHair
{
    public class GradientHairModSettings :ModSettings
    {
        public float chanceMale = 0.02f;
        public float chanceFemale = 1.0f;
        public bool enable = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref enable, "enable", true);
            Scribe_Values.Look(ref chanceMale, "chanceMale", 0.02f);
            Scribe_Values.Look(ref chanceFemale, "chanceFemale", 1.0f);
        }
    }
}