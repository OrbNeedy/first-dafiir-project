using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.DrawTypes
{
    public class BackAndFront : Drawing
    {
        public override void Postdraw(float centerX, float centerY, float rotation, float time, float ai1, 
            float ai2, float r, float g, float b, float a, BulletField field)
        {
            DrawEngine.AddDraw((int)ai2, centerX, centerY, rotation, r: r, g: g, b: b, a: a, layer: -1);
            DrawEngine.AddDraw((int)ai1, centerX, centerY, rotation, layer: 1);
        }
    }
}
