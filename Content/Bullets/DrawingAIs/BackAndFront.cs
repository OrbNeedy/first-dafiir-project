using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.DrawingAIs
{
    public class BackAndFront : Drawing
    {
        public override void Postdraw(float centerX, float centerY, float directionX, float directionY,
            float sizeX, float sizeY, float rotation, float time, ref float ai1, ref float ai2,
            float r, float g, float b, float a, BulletField field)
        {
            DrawEngine.AddDraw((int)ai2, centerX, centerY, rotation, r: r, g: g, b: b, a: a, layer: -1);
            DrawEngine.AddDraw((int)ai1, centerX, centerY, rotation, layer: 1);
        }
    }
}
