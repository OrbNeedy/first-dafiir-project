using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content
{
    public class Player
    {
        public Vector2 position;
        public Emitter[] emitters;
        private int emitterOrbit = 0;
        public Aabb hitbox;

        public bool shooting = false;
        public Vector2 velocity = new Vector2();
        public BulletField field;
        private float speed = 6;
        public bool focus = false;
        public int iFrames = 0;

        public Player(BulletField field)
        {
            emitters = new Emitter[] { new Emitter(new PlayerFocusShot(field), 5, 1) };
            this.field = field;
            hitbox = new Aabb(new Vector3(position.X-10, position.Y-10, 0), new Vector3(20, 20, 1));
        }

        public void HandleInputs()
        {
            if (Input.IsActionPressed("accept"))
            {
                shooting = true;
            }

            if (Input.IsActionPressed("shift"))
            {
                focus = true;
            }

            if (Input.IsActionPressed("right"))
            {
                velocity.X++;
            }
            if (Input.IsActionPressed("left"))
            {
                velocity.X--;
            }
            if (Input.IsActionPressed("up"))
            {
                velocity.Y--;
            }
            if (Input.IsActionPressed("down"))
            {
                velocity.Y++;
            }
        }

        public void Update(double delta, Rect2 margin)
        {
            velocity = velocity.Normalized();

            float tempSpeed = focus ? speed * 2 / 5 : speed;

            position += velocity * tempSpeed;
            if (position.X < margin.Position.X)
            {
                position.X = margin.Position.X;
            }
            if (position.X > margin.Position.X + margin.Size.X)
            {
                position.X = margin.Position.X + margin.Size.X;
            }

            if (position.Y < margin.Position.Y)
            {
                position.Y = margin.Position.Y;
            }
            if (position.Y > margin.Position.Y + margin.Size.Y)
            {
                position.Y = margin.Position.Y + margin.Size.Y;
            }

            bool finalShoot = shooting;
            foreach (Emitter emitter in emitters)
            {
                emitter.Update();
                emitter.position = position;
                if (shooting)
                {
                    finalShoot = finalShoot && !emitter.Shoot(field.Margin, this, 1, 0, emitterOrbit);
                }
            }
            shooting = finalShoot;

            // Subtract half of the hitbox's size to it's position so it will be in the middle
            hitbox = new Aabb(new Vector3(position.X - 5, position.Y - 5, 0), new Vector3(10, 10, 1));

            velocity.X = 0;
            velocity.Y = 0;
            focus = false;
            emitterOrbit++;
            if (iFrames > 0) iFrames--;
        }

        public void OnHit(float damage)
        {
            iFrames = 60;
        }

        public void Draw()
        {
            Color color = new Color(1, 1, 1);
            if (iFrames > 0) color = new Color(7f, 0.2f, 0.2f, 0.5f);
            DrawEngine.AddDraw((int)Textures.Player, position.X, position.Y, 0, layer: -1, r: color.R,
                g: color.G, b: color.B, a: color.A);
        }
    }
}
