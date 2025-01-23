using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Utils;
using System;

namespace PathtoDarkSide.Content.Enemies
{
    public class EnemyDeathEventArgs : EventArgs
    {
        public int points;

        public EnemyDeathEventArgs(int points)
        {
            this.points = points;
        }
    }

    public class Enemy
    {
        public Vector2 position;
        public Aabb hitbox;

        private int texture2DIndex;
        private int frame = 0;
        private int frameCounter = 0;

        public float lifePoints;
        private Emitter[] emitters;
        private BulletField field;

        public event EventHandler<EnemyDeathEventArgs> Death;

        public Enemy(Vector2 position, int texture, float life, Emitter[] emitters, BulletField field)
        {
            this.position = position;
            texture2DIndex = texture;
            lifePoints = life;
            this.emitters = emitters;
            this.field = field;
            hitbox = new Aabb(new Vector3(position.X-25, position.Y-25, 0), new Vector3(50, 50, 1));
        }

        public void Update()
        {
            frameCounter++;
            foreach (var emitter in emitters)
            {
                emitter.Update();
                emitter.Shoot(field, 1, 1);
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
        }

        public void OnHit(float damage)
        {
            lifePoints -= damage;
            if (lifePoints <= 0)
            {
                OnDeath(new EnemyDeathEventArgs(10));
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
