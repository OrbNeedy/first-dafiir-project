using Godot;
using PathtoDarkSide.Content.Enemies;
using PathtoDarkSide.Content.Utils;
using System;

namespace PathtoDarkSide.Content.Bullets.Emmiters.Patterns
{
    public class Pattern
    {
        public event EventHandler<BulletSpawnEventArgs> BulletSpawn;
        /*Rules of all enemy patterns:
         1. The pattern can only start if the emitter is on screen, it can however increase timers
         2. The pattern can't shoot bullets if the emitter is not in the leftmost part of the screen*/
        /*Exceptions:
         Bosses*/
        public Pattern()
        {
            BulletSpawn += Main.bulletField.HandleBulletSpawn;
        }

        public virtual bool Shoot(Vector2 position, int cycle, int maxCycle, Vector2 target, int dificulty, 
            int speed, params float[] parameters)
        {
            int bullets = 10 * dificulty;
            Vector2 direction = position.DirectionTo(target);
            for (int i = 0; i < bullets; i++)
            {
                SpawnBullet(this, new BulletSpawnEventArgs(position, direction, 0, 1 + (speed), 
                    visualParam1: (int)Textures.Inner, sizeX: 20, sizeY: 20));
                direction = direction.Rotated(Mathf.Tau / bullets);
            }
            return true;
        }

        protected void SpawnBullet(object? sender, BulletSpawnEventArgs e)
        {
            BulletSpawn?.Invoke(sender, e);
        }

        public virtual bool CanShoot(Vector2 position)
        {
            bool val = position.X > Main.bulletField.Margin.Position.X + 100 &&
                position.X < Main.bulletField.Margin.Size.X + Main.bulletField.Margin.Position.X &&
                position.Y < Main.bulletField.Margin.Size.Y + Main.bulletField.Margin.Position.Y &&
                position.Y > Main.bulletField.Margin.Position.Y;
            return val;
        }
    }
}
