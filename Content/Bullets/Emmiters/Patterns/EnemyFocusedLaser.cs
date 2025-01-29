using Godot;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.Emmiters.Patterns
{
    public class EnemyFocusedLaser : Pattern
    {
        public EnemyFocusedLaser() : base()
        {
        }

        public override bool Shoot(Vector2 position, int cycle, int maxCycle, Vector2 target, int dificulty, 
            int speed, params float[] parameters)
        {
            Vector2 direction = position.DirectionTo(target);
            Vector2 finalPosition = position + (direction * 1000);

            SpawnBullet(this, new BulletSpawnEventArgs(position, direction, direction.Angle(), 0, 
                sizeX: finalPosition.X, sizeY: finalPosition.Y, width: 0,
                script: (int)BulletTypes.Laser, drawScript: (int)DrawTypes.Laser, penetration: -1,
                visualParam1: 0.05f, shape: (int)Shapes.Line, ai1: 6));
            return true;
        }
    }
}
