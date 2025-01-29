using Godot;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.DrawingAIs
{
    public class Drawing
    {
        public virtual void Postdraw(float centerX, float centerY, float directionX, float directionY, 
            float sizeX, float sizeY, float rotation, float time, ref float ai1, ref float ai2, 
            float r, float g, float b, float a)
        {
            DrawEngine.AddDraw((int)ai1, centerX, centerY, rotation, r: r, g: g, b: b, a: a);
        }
    }
}
