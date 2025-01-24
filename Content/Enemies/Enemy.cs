using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Enemies.AI;
using PathtoDarkSide.Content.Enemies.OffensiveAI;
using PathtoDarkSide.Content.Utils;
using System;

namespace PathtoDarkSide.Content.Enemies
{
    public class EnemyDeathEventArgs : EventArgs
    {
        public int points;
        public bool notify;

        public EnemyDeathEventArgs(int points, bool notify = false)
        {
            this.points = points;
            this.notify = notify;
        }
    }

    public class Enemy
    {
        public Vector2 position;
        public Aabb hitbox;
        public bool imuneToStopTime;

        private int texture2DIndex;
        private int frame = 0;
        private int frameCounter = 0;

        public float lifePoints;
        public bool dead = false;
        private Emitter[] emitters;
        private BulletField field;
        private Move movement;
        private Attack attack;

        public event EventHandler<EnemyDeathEventArgs> Death;

        public Enemy(int texture, float life, BulletField field, Move movement, Attack attack)
        {
            texture2DIndex = texture;
            lifePoints = life;
            this.field = field;
            hitbox = new Aabb(new Vector3(position.X-25, position.Y-25, 0), new Vector3(50, 50, 1));
            this.movement = movement;
            movement.Initialize(1, field, ref position);
            this.attack = attack;
            attack.Initialize(1, 1, field, ref emitters);
        }

        public void Update()
        {
            frameCounter++;

            movement.Update(1, field.Margin, field.Player, ref position);
            attack.Update(1, 1, field.Margin, field.Player, this, ref emitters);

            foreach (var emitter in emitters)
            {
                emitter.Update();
                emitter.Shoot(field.Margin, field.Player, 1, 1);
            }

            hitbox = new Aabb(new Vector3(position.X-25, position.Y-25, 0), new Vector3(50, 50, 1));

            if (frameCounter > 20)
            {
                frame++;
                frameCounter = 0;
                if (frame >= 3)
                {
                    frame = 0;
                }
            }

            if (movement.DeathCondition(position, field) && !dead)
            {
                lifePoints = 0;
                dead = true;
                OnDeath(new EnemyDeathEventArgs(0));
            }
        }

        public void OnHit(float damage)
        {
            // The enemy can only take damage and die of it if it's inside the boundaries of the bullet field plus
            // 20 pixels, but a sound should still play if it's hit
            if (position.X < field.Margin.Position.X - 20 ||
                position.X > field.Margin.Size.X + field.Margin.Position.X + 20 ||
                position.Y < field.Margin.Position.Y - 20 ||
                position.Y > field.Margin.Size.Y + field.Margin.Position.Y + 20) return;
            lifePoints -= damage;
            if (lifePoints <= 0)
            {
                dead = true;
                OnDeath(new EnemyDeathEventArgs(10, true));
            }
        }

        private void OnDeath(EnemyDeathEventArgs e)
        {
            Death?.Invoke(this, e);
        }

        public void Draw()
        {
            DrawEngine.AddDraw(texture2DIndex, position.X, position.Y, 0, frame: frame);
        }
    }
}
