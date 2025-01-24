using Godot;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.Emmiters.Patterns
{
    public class Pattern
    {
        /*Rules of all enemy patterns:
         1. The pattern can only start if the emitter is on screen, it can however increase timers
         2. The pattern can't shoot bullets if the emitter is not in the leftmost part of the screen*/
        public virtual bool Shoot(BulletField field, Vector2 position, int cycle, int maxCycle, Vector2 target, 
            int dificulty, int speed, params float[] parameters)
        {
            int bullets = 10 * dificulty;
            Vector2 direction = position.DirectionTo(target);
            for (int i = 0; i < bullets; i++)
            {
                field.AddProjectile(position, direction, 0, 1 + (speed), visualParam1: (int)Textures.Inner, 
                    sizeX: 15, sizeY: 15);
                direction = direction.Rotated(MathConsts.TwoPi / bullets);
            }
            return true;
        }

        public virtual bool CanShoot(Vector2 position, BulletField field)
        {
            return position.X > field.Margin.Position.X + 100 ||
                position.X < field.Margin.Size.X + field.Margin.Position.X ||
                position.Y > field.Margin.Size.Y + field.Margin.Position.Y ||
                position.Y < field.Margin.Position.Y;
        }
    }
}
