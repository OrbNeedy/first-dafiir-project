using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;

namespace PathtoDarkSide.Content.Enemies.OffensiveAI
{
    public class AttackBehavior
    {
        public int type;

        public AttackBehavior(int type)
        {
            this.type = type;
        }

        public virtual void Initialize(int difficulty, int speed, ref Emitter[] emitters)
        {
            emitters = new Emitter[] { new Emitter(new Pattern(), 120, 5) };
        }

        public virtual void Update(int difficulty, int speed, Enemy enemy, ref Emitter[] emitters)
        {
            foreach (Emitter emitter in emitters)
            {
                emitter.position = enemy.position;
                if (type == 0)
                {
                    emitter.target = enemy.position + new Vector2(1, 0).
                        Rotated(Main.randomNumberGenerator.RandfRange(0, Mathf.Tau));
                }
            }
        }
    }
}
