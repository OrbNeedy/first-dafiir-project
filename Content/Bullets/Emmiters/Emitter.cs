using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;

namespace PathtoDarkSide.Content.Bullets.Emmiters
{
    public class Emitter
    {
        public Vector2 position = new Vector2();
        public Vector2 target;
        public int timer = 0;
        public int maxTimer = 60;
        public int cycle = 0;
        public int maxCycle = 2;
        public Pattern pattern;

        public Emitter(Pattern pattern, int maxTimer, int maxCycle) 
        { 
            this.pattern = pattern;
            this.maxTimer = maxTimer;
            this.maxCycle = maxCycle;
        }

        public void Update()
        {
            if (timer > 0) timer--;
        }

        public bool Shoot(BulletField bulletField, int difficulty, int speed, params float[] parameters)
        {
            if (timer <= 0)
            {
                bool returnValue = pattern.CanShoot(position, bulletField);
                if (returnValue)
                {
                    pattern.Shoot(bulletField, position, cycle, maxCycle, target, difficulty, speed, parameters);
                    timer = maxTimer;
                    cycle++;
                    if (cycle >= maxCycle) cycle = 0;
                }
                return returnValue;
            }
            return false;
        }
    }
}
