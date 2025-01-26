using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;

namespace PathtoDarkSide.Content.Enemies.OffensiveAI
{
    public class ConcentratedLaser : Attack
    {
        public ConcentratedLaser(int type) : base(type)
        {
        }

        public override void Initialize(int difficulty, int speed, BulletField field, ref Emitter[] emitters)
        {
            emitters = new Emitter[] { new Emitter(new EnemyFocusedLaser(field), 300, 2) };
        }

        public override void Update(int difficulty, int speed, Rect2 margin, Player player, Enemy enemy, 
            ref Emitter[] emitters)
        {
            foreach (Emitter emitter in emitters)
            {
                emitter.position = enemy.position;
                emitter.target = player.position;
            }
        }
    }
}
