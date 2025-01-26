using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;
using PathtoDarkSide.Content.Enemies;
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
                    finalShoot = finalShoot && !emitter.Shoot(field.Margin, this, 1, 0, emitterOrbit, 1);
                }
            }
            shooting = finalShoot;

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
            DrawEngine.AddDraw((int)Textures.Hitbox, position.X, position.Y, 0, layer: 0);

        }

        public bool Collided(float[] bullet)
        {
            // Special Line collision if statement
            if (bullet[(int)BulletAttributes.Shape] == (int)Shapes.Line)
            {
                bool t = Collision.LinevCircle(
                    new Vector2(bullet[(int)BulletAttributes.CenterX], bullet[(int)BulletAttributes.CenterY]),
                    new Vector2(bullet[(int)BulletAttributes.SizeX], bullet[(int)BulletAttributes.SizeY]),
                    position, hitbox.Size.X/2, bullet[(int)BulletAttributes.Width]);
                if (t)
                {
                    //GD.Print($"CvL collision center: {position}");
                }
                return t;
            }

            bool aabbvAabbCheck = Collision.AabbvAabb(hitbox, new Aabb(bullet[(int)BulletAttributes.CenterX] -
                (bullet[(int)BulletAttributes.SizeX] / 2), bullet[(int)BulletAttributes.CenterY] -
                (bullet[(int)BulletAttributes.SizeY] / 2), 0,
                bullet[(int)BulletAttributes.SizeX], bullet[(int)BulletAttributes.SizeY], 1));
            if (aabbvAabbCheck)
            {
                switch (bullet[(int)BulletAttributes.Shape])
                {
                    case (int)Shapes.Rectangle:
                        GD.Print($"Player hit by a rectangle");
                        return true;
                    case (int)Shapes.Circle:
                        bool t = Collision.CirclevCircle(position, hitbox.Size.X,
                            new Vector2(bullet[(int)BulletAttributes.CenterX], bullet[(int)BulletAttributes.CenterY]),
                            bullet[(int)BulletAttributes.Width]);
                        if (t)
                        {
                            GD.Print($"Player hit by a circle");
                        }
                        return t;
                }
            }
            return false;
        }

        public bool Collided(Enemy enemy)
        {
            // Special Line collision if statement
            // This may go unused since I doubt any line shaped enemies will appear
            if (enemy.shape == (int)Shapes.Line)
            {
                return Collision.LinevCircle(enemy.position, enemy.hurtboxSize,
                    position, hitbox.Size.X/2);
            }

            if (Collision.AabbvAabb(hitbox, enemy.hurtbox))
            {
                switch (enemy.shape)
                {
                    case (int)Shapes.Rectangle:
                        return true;
                    case (int)Shapes.Circle:
                        return Collision.CirclevCircle(position, hitbox.Size.X, enemy.position, enemy.hurtboxSize.X);
                }
            }
            return false;
        }
    }
}
