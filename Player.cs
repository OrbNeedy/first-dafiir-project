using Godot;
using PathtoDarkSide.Content;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide
{
    public class Player
    {
        public Vector2 position;
        public Emitter[] emitters;
        private int emitterOrbit = 0;

        public bool shooting = false;
        public Vector2 velocity = new Vector2();
        public BulletField field;
        private float speed = 6;
        public bool focus = false;
        public int iFrames = 0;

        public Player(BulletField field)
        {
            emitters = new Emitter[] { new Emitter(position, new PlayerFocusShot(), 5, 1) };
            this.field = field;
            field.PlayerHit += HandleOnHit;
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

            float tempSpeed = focus? speed*2/5 : speed;

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
                    finalShoot = finalShoot && !emitter.Shoot(field, 1, 0, emitterOrbit);
                }
            }
            shooting = finalShoot;

            if (iFrames <= 0)
            {
                field.Colliders[(int)HitLayers.Player].Add(new Aabb(
                    new Vector3(position.X - 10, position.Y - 10, 0), new Vector3(20, 20, 1)));
            }

            velocity.X = 0;
            velocity.Y = 0;
            focus = false;
            emitterOrbit++;
            if (iFrames > 0) iFrames--;
        }

        public void HandleOnHit(object sender, PlayerHitEventArgs e)
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
