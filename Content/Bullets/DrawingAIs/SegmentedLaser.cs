using Godot;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.DrawingAIs
{
    public class SegmentedLaser : Drawing
    {
        public override void Postdraw(float centerX, float centerY, float directionX, float directionY,
            float sizeX, float sizeY, float rotation, float time, ref float ai1, ref float ai2,
            float r, float g, float b, float a)
        {
            int frameLength = TexturesTable.LoadedTextures[(int)Textures.Laser].Length;
            
            if (time == 45)
            {
                ai1 = 2;
            }
            if (time > 45 && time < 120 && ai1 < frameLength && time%18 == 0)
            {
                ai1++;
            }
            if (time > 120 && ai1 > 1 && time%12 == 0)
            {
                ai1--;
            }

            Vector2 position = new Vector2(centerX, centerY);
            Vector2 finalPosition = new Vector2(sizeX, sizeY);
            float length = position.DistanceTo(finalPosition);

            for (int i = 0; i < length; i += 20)
            {
                if ((time < 45 || time > 168) && time % 12 == 0)
                {
                    ai1 = Main.randomNumberGenerator.RandiRange(0, 1);
                }
                DrawEngine.AddDraw((int)Textures.Laser, position.X, position.Y, rotation, frame: (int)ai1);
                position.X += directionX * 20;
                position.Y += directionY * 20;
            }
        }
    }
}
