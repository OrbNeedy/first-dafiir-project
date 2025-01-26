using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;

namespace PathtoDarkSide.Content.Bullets.AIs
{
    public class Laser : Bullet
    {
        public override void UpdateBullet(ref float centerX, ref float centerY, ref float directionX, 
            ref float directionY, ref float rotation, ref float speed, ref float shape, ref float sizeX, 
            ref float sizeY, ref float width, ref float time, ref float drawScript, ref float layer, ref float damage, 
            ref float ai1, ref float ai2, ref float ai3, ref float ai4, BulletField field)
        {
            if (time <= 0)
            {
                width = 0;
            }
            if (time > 45 && width < ai1)
            {
                width += ai1/50;
            }
            GD.Print($"Width: {width}");
            GD.Print($"Time: {time}");

            if (time > 120)
            {
                if (width < 0)
                {
                    centerX = field.Margin.Size.X * 1000;
                    centerY = field.Margin.Size.Y * 1000;
                    sizeX = centerX;
                    sizeY = centerY;
                }
                width -= ai1/25;
            }
        }
    }
}
