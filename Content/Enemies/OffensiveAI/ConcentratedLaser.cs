using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;

namespace PathtoDarkSide.Content.Enemies.OffensiveAI
{
    public class ConcentratedLaser : AttackBehavior
    {
        public ConcentratedLaser(int type) : base(type)
        {
        }

        public override void Initialize(int difficulty, int speed, ref Emitter[] emitters)
        {
            emitters = new Emitter[] { new Emitter(new EnemyFocusedLaser(), 300, 2) };
        }

        public override void Update(int difficulty, int speed, Enemy enemy, 
            ref Emitter[] emitters)
        {
            foreach (Emitter emitter in emitters)
            {
                emitter.position = enemy.position;
                emitter.target = Main.bulletField.Player.position;
            }
        }
    }
}
