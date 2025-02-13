﻿using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Enemies.AI;
using PathtoDarkSide.Content.Enemies.DeathAI;
using PathtoDarkSide.Content.Enemies.OffensiveAI;
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
        public Aabb hitbox; // The collider for this enemy vs bullets
        public Vector2 hitboxSize;
        public int shape; // Only the hitbox has a shape, the hurtbox is always a square
        public Aabb hurtbox; // The collider for this enemy vs the player
        public Vector2 hurtboxSize;
        public bool immuneToStopTime;

        private int texture2DIndex;
        private int frame = 0;
        public int maxFrameCount;
        private int frameCount = 0;

        public float lifePoints;
        public bool dead = false;
        public int iFrames = 0;

        private Emitter[] emitters;
        public float damageValue;
        private Behavior behavior;
        private AttackBehavior attackBehavior;
        private DeathBehavior? deathBehavior;

        public event EventHandler<EnemyDeathEventArgs> Death;

        public Enemy(int texture, float life, Behavior behavior, DeathBehavior? deathBehavior, 
            AttackBehavior attackBehavior, Vector2 hitboxSize, Vector2 hurtboxSize, int maxFrameCount = 18, 
            int hitboxShape = 0)
        {
            texture2DIndex = texture;
            lifePoints = life;
            hitbox = new Aabb(new Vector3(position.X - (hitboxSize.X / 2), position.Y - (hitboxSize.Y / 2), 0),
                new Vector3(hitboxSize.X, hitboxSize.Y, 1));
            hurtbox = new Aabb(new Vector3(position.X - (hurtboxSize.X / 2), 
                position.Y - (hurtboxSize.Y / 2), 0), new Vector3(hurtboxSize.X, hurtboxSize.Y, 1));
            this.behavior = behavior;
            behavior.Initialize(1, this);
            this.attackBehavior = attackBehavior;
            this.deathBehavior = deathBehavior;
            attackBehavior.Initialize(1, 1, ref emitters);
            this.shape = hitboxShape;
            this.hitboxSize = hitboxSize;
            this.hurtboxSize = hurtboxSize;
            this.maxFrameCount = maxFrameCount;
        }

        public void Update(int difficulty, int speed)
        {
            frameCount++;

            behavior.Update(speed, this);
            attackBehavior.Update(difficulty, speed, this, ref emitters);

            foreach (var emitter in emitters)
            {
                emitter.Update();
                emitter.Shoot(1, 1, damageValue);
            }

            hitbox.Position = new Vector3(position.X, position.Y, 0);
            hurtbox.Position = new Vector3(position.X, position.Y, 0);

            if (iFrames > 0)
            {
                iFrames--;
            }

            if (frameCount > maxFrameCount)
            {
                frame++;
                frameCount = 0;
                if (frame >= TexturesTable.LoadedTextures[texture2DIndex].Length) 
                {
                    frame = 0;
                }
            }

            if (behavior.DeathCondition(position) && !dead)
            {
                lifePoints = 0;
                dead = true;
                deathBehavior?.DeathOccured(this);
                OnDeath(new EnemyDeathEventArgs(0));
            }
        }

        public void OnHit(float damage, Rect2 margin, bool giveIframes = false)
        {
            // The enemy can only take damage and die of it if it's inside the boundaries of the bullet field plus
            // 20 pixels, but a sound should still play if it's hit
            if (position.X < margin.Position.X - 20 ||
                position.X > margin.Size.X + margin.Position.X + 20 ||
                position.Y < margin.Position.Y - 20 ||
                position.Y > margin.Size.Y + margin.Position.Y + 20 ||
                iFrames > 0) return;
            lifePoints -= damage;
            if (giveIframes) iFrames = 7;
            if (lifePoints <= 0)
            {
                dead = true;
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

        public bool Collided(float[] bullet)
        {
            // Special Line collision if statement
            if (bullet[(int)BulletAttributes.Shape] == (int)Shapes.Line)
            {
                return Collision.AabbvLine(hitbox, 
                    new Vector2(bullet[(int)BulletAttributes.CenterX], bullet[(int)BulletAttributes.CenterY]),
                    new Vector2(bullet[(int)BulletAttributes.SizeX], bullet[(int)BulletAttributes.SizeY]),
                    bullet[(int)BulletAttributes.Width]);
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
                        return true;
                    case (int)Shapes.Circle:
                        return Collision.AabbvCircle(hitbox,
                            new Vector2(bullet[(int)BulletAttributes.CenterX], bullet[(int)BulletAttributes.CenterY]),
                            bullet[(int)BulletAttributes.Width]);
                }
            }
            return false;
        }
    }
}
