using PathtoDarkSide.Content.Bullets.DrawingAIs;
using PathtoDarkSide.Content.Visuals;

namespace PathtoDarkSide.Content.Utils
{
    public static class VisualTables
    {
        public static Drawing[] DrawingAIs = new Drawing[] { new Drawing(), new BackAndFront(), 
            new SegmentedLaser() 
        };
        public static VFX[] VisualEffects = new VFX[] { new VFX(), new FadeoutShrink() };
    }
}
