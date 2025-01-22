using Godot;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.Emmiters.Patterns
{
    public class Pattern
    {
        public virtual void Shoot(BulletField field, Vector2 position, int cycle, int maxCycle, Vector2 target, 
            int dificulty, int speed, params float[] parameters)
        {
            int bullets = 10 * dificulty;
            Vector2 direction = position.DirectionTo(target);
            for (int i = 0; i < bullets; i++)
            {
                field.AddProjectile(position, direction, 0, 1 + (speed));
                direction = direction.Rotated(MathConsts.TwoPi/bullets);
            }
        }
    }
}
