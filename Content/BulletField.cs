using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Utils;
using static PathtoDarkSide.Content.Utils.VisualTables;
using System;
using System.Collections.Generic;
using static PathtoDarkSide.Content.Utils.TexturesTable;

public enum BulletAttributes
{
    CenterX, 
    CenterY,
    DirectionX, 
    DirectionY,
    Rotation, 
    Speed, 
    Shape, 
    SizeX, 
    SizeY,
    Time, 
    AI, 
    DrawAI, 
    HitLayer,
    Damage,
    Param1, 
    Param2, 
    Param3, 
    Param4, 
    VisualParam1, 
    VisualParam2,
    R,
    G, 
    B, 
    A
}

public enum EffectAttributes
{
    Texture,
    PositionX,
    PositionY,
    Rotation, 
    ScaleX, 
    ScaleY,
    R, G, B, A,
    Layer,
    Frame,
    AI,
    Time,
    Param1,
    Param2,
}

namespace PathtoDarkSide.Content {
    public class PlayerHitEventArgs : EventArgs
    {
        public float damage { get; }
        public PlayerHitEventArgs(float damage)
        {
            this.damage = damage;
        }
    }

    public class BulletField
    {
        Predicate<float[]> deleteConditions;
        public event EventHandler<PlayerHitEventArgs> PlayerHit;

        public Rect2 Margin;
        public static Bullet[] BulletAIs = new Bullet[] { new Bullet() };
        public List<float[]> ActiveBullets = new List<float[]>();
        public Dictionary<int, List<Aabb>> Colliders = new Dictionary<int, List<Aabb>>()
        {
            [(int)HitLayers.Player] = new List<Aabb>(),
            [(int)HitLayers.Enemy] = new List<Aabb>()
        };
        
        public BulletField(Rect2 margin) 
        {
            deleteConditions = DeleteConditions;
            this.Margin = margin;
        }

        public void Update(double delta)
        {
            foreach (var bullet in ActiveBullets)
            {
                if ((int)bullet[(int)BulletAttributes.DrawAI] < 0 ||
                    (int)bullet[(int)BulletAttributes.DrawAI] >= DrawingAIs.Length)
                {
                    bullet[(int)BulletAttributes.DrawAI] = 0;
                }

                if ((int)bullet[(int)BulletAttributes.AI] < 0 ||
                    (int)bullet[(int)BulletAttributes.AI] >= BulletAIs.Length)
                {
                    bullet[(int)BulletAttributes.AI] = 0;
                }
                BulletAIs[(int)bullet[(int)BulletAttributes.AI]].UpdateBullet(ref bullet[(int)BulletAttributes.CenterX],
                    ref bullet[(int)BulletAttributes.CenterY], ref bullet[(int)BulletAttributes.DirectionX],
                    ref bullet[(int)BulletAttributes.DirectionY], ref bullet[(int)BulletAttributes.Rotation],
                    ref bullet[(int)BulletAttributes.Speed], ref bullet[(int)BulletAttributes.Shape],
                    ref bullet[(int)BulletAttributes.SizeX], ref bullet[(int)BulletAttributes.SizeY],
                    ref bullet[(int)BulletAttributes.Time], ref bullet[(int)BulletAttributes.DrawAI],
                    ref bullet[(int)BulletAttributes.HitLayer], ref bullet[(int)BulletAttributes.Damage],
                    ref bullet[(int)BulletAttributes.Param1], ref bullet[(int)BulletAttributes.Param2],
                    ref bullet[(int)BulletAttributes.Param3], ref bullet[(int)BulletAttributes.Param4], this);

                // Postdraw bullet
                DrawingAIs[(int)bullet[(int)BulletAttributes.DrawAI]].Postdraw(bullet[(int)BulletAttributes.CenterX],
                    bullet[(int)BulletAttributes.CenterY], bullet[(int)BulletAttributes.Rotation],
                    bullet[(int)BulletAttributes.Time], bullet[(int)BulletAttributes.VisualParam1], 
                    bullet[(int)BulletAttributes.VisualParam2], bullet[(int)BulletAttributes.R], 
                    bullet[(int)BulletAttributes.G], bullet[(int)BulletAttributes.B], 
                    bullet[(int)BulletAttributes.A], this);
                bullet[(int)BulletAttributes.Time] += 1;
            }

            ActiveBullets.RemoveAll(deleteConditions);

            foreach (var col in Colliders)
            {
                col.Value.Clear();
            }
        }

        public void AddProjectile(Vector2 position, Vector2 direction, float rotation, float speed, float shape = 0,
            float sizeX = 10, float sizeY = 10, float script = 0, float drawScript = 0, 
            float hitLayer = (int)HitLayers.Player, float damage = 1, float ai1 = 0, float ai2 = 0, float ai3 = 0, 
            float ai4 = 0, float visualParam1 = 0, float visualParam2 = 0, float r = 1, float g = 1, float b = 1, 
            float a = 1)
        {
            direction = direction.Normalized();

            float[] bullet = new float[] { position.X, position.Y, direction.X, direction.Y, rotation, speed, shape,
            sizeX, sizeY, 0, script, drawScript, hitLayer, damage, ai1, ai2, ai3, ai4, visualParam1, visualParam2, r, 
            g, b, a };

            ActiveBullets.Add(bullet);
        }

        private bool Collide(float[] bullet, Aabb target)
        {
            Aabb bulletAabb = new Aabb(bullet[(int)BulletAttributes.CenterX] - 
                (bullet[(int)BulletAttributes.SizeX] / 2), bullet[(int)BulletAttributes.CenterY] - 
                (bullet[(int)BulletAttributes.SizeY] / 2), 0,
                bullet[(int)BulletAttributes.SizeX], bullet[(int)BulletAttributes.SizeY], 1);
            if (bulletAabb.Intersects(target))
            {
                switch (bullet[(int)BulletAttributes.Shape])
                {
                    case 0:
                        return true;
                    case 1:
                        Vector2 targetCenter = new Vector2(target.Position.X + (target.Size.X / 2), 
                            target.Position.Y + (target.Size.X / 2));
                        float deltaX = (float)Math.Pow(targetCenter.X - bullet[(int)BulletAttributes.CenterX], 2);
                        float deltaY = (float)Math.Pow(targetCenter.Y - bullet[(int)BulletAttributes.CenterY], 2);
                        float radius = (float)Math.Pow((bullet[(int)BulletAttributes.SizeX] / 2) + 
                            (target.Size.X / 2), 2);
                        return Math.Sqrt(deltaX + deltaY) < Math.Sqrt(radius);
                }
            }
            return false;
        }

        public void ClearScreen()
        {
            ActiveBullets.Clear();
        }

        public bool DeleteConditions(float[] bullet)
        {
            if (CheckOutOfBounds(bullet)) return true;
            foreach (Aabb aabb in Colliders[(int)bullet[(int)BulletAttributes.HitLayer]])
            {
                if (Collide(bullet, aabb))
                {
                    if ((int)bullet[(int)BulletAttributes.HitLayer] == (int)HitLayers.Player)
                    {
                        OnPlayerHit(new PlayerHitEventArgs(bullet[(int)BulletAttributes.Damage]));
                    }
                    return true;
                }
            }
            return false;
        }

        private void OnPlayerHit(PlayerHitEventArgs e)
        {
            PlayerHit?.Invoke(this, e);
        }

        public bool CheckOutOfBounds(float[] bullet)
        {
            int limit = 100;
            return bullet[(int)BulletAttributes.CenterX] >= Margin.Position.X + Margin.Size.X + limit || 
                bullet[(int)BulletAttributes.CenterX] <= Margin.Position.X - limit ||
                bullet[(int)BulletAttributes.CenterY] >= Margin.Position.Y + Margin.Size.Y + limit || 
                bullet[(int)BulletAttributes.CenterY] <= Margin.Position.Y - limit;
        }

        public int GetBulletCount()
        {
            return ActiveBullets.Count;
        }
    }
}
