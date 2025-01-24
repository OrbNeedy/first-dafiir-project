using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Enemies.AI;
using PathtoDarkSide.Content.Enemies.OffensiveAI;
using PathtoDarkSide.Content.Utils;
using static PathtoDarkSide.Content.Utils.VisualTables;
using System;
using System.Collections.Generic;
using PathtoDarkSide.Content.Enemies;

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

namespace PathtoDarkSide.Content
{
    public class BulletSpawnEventArgs : EventArgs
    {
        /*Vector2 position, Vector2 direction, float rotation, float speed, float shape = 0,
            float sizeX = 10, float sizeY = 10, float script = 0, float drawScript = 0, 
            float hitLayer = (int)HitLayers.Player, float damage = 1, float ai1 = 0, float ai2 = 0, float ai3 = 0, 
            float ai4 = 0, float visualParam1 = 0, float visualParam2 = 0, float r = 1, float g = 1, float b = 1, 
            float a = 1*/
        public Vector2 position;
        public Vector2 direction;
        public float rotation;
        public float speed;
        public float shape;
        public float sizeX;
        public float sizeY;
        public float script;
        public float drawScript;
        public float hitLayer;
        public float damage;
        public float ai1;
        public float ai2;
        public float ai3;
        public float ai4;
        public float visualParam1;
        public float visualParam2;
        public float r;
        public float g;
        public float b;
        public float a;

        public BulletSpawnEventArgs(Vector2 position, Vector2 direction, float rotation, float speed, 
            float shape = 0, float sizeX = 10, float sizeY = 10, float script = 0, float drawScript = 0,
            float hitLayer = (int)HitLayers.Player, float damage = 1, float ai1 = 0, float ai2 = 0, float ai3 = 0,
            float ai4 = 0, float visualParam1 = 0, float visualParam2 = 0, float r = 1, float g = 1, float b = 1,
            float a = 1)
        {
            this.position = position;
            this.direction = direction;
            this.rotation = rotation;
            this.speed = speed;
            this.shape = shape;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.script = script;
            this.drawScript = drawScript;
            this.hitLayer = hitLayer;
            this.damage = damage;
            this.ai1 = ai1;
            this.ai2 = ai2;
            this.ai3 = ai3;
            this.ai4 = ai4;
            this.visualParam1 = visualParam1;
            this.visualParam2 = visualParam2;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
    }

    public class BulletField
    {
        Predicate<float[]> deleteConditions;

        public Rect2 Margin;
        public Main main;
        public static Bullet[] BulletAIs = new Bullet[] { new Bullet() };
        public List<float[]> ActiveBullets = new List<float[]>();

        public List<Enemy> ActiveEnemies = new List<Enemy>();
        public Player Player;
        public bool paused = false;
        public bool stoppedTime = false;
        public bool hidePlayer = false;

        public BulletField(Rect2 margin, Main main) 
        {
            deleteConditions = DeleteConditions;
            Margin = margin;
            this.main = main;
            Player = new Player(this); 
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

                if (!stoppedTime && !paused)
                {
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
                }

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

            foreach (Enemy enemy in ActiveEnemies)
            {
                if ((!stoppedTime && !enemy.imuneToStopTime) && !paused)
                {
                    enemy.Update();
                }
                enemy.Draw();
            }

            ActiveEnemies.RemoveAll((x) => x.dead);

            Player.HandleInputs();
            if (!stoppedTime && !paused)
            {
                Player.Update(delta, Margin);
            }
            Player.Draw();

            if (Main.randomNumberGenerator.RandiRange(0, 100) < 2)
            {
                AddEnemy(Textures.Pixie, 60, new Move(-1), new Attack(0));
            }
        }

        public void HandleBulletSpawn(object sender, BulletSpawnEventArgs e)
        {
            AddProjectile(e.position, e.direction, e.rotation, e.speed, e.shape, e.sizeX, e.sizeY, e.script, 
                e.drawScript, e.hitLayer, e.damage, e.ai1, e.ai2, e.ai3, e.ai4, e.visualParam1, e.visualParam2, 
                e.r, e.g, e.b, e.a);
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

        public void AddEnemy(Textures texture, float life, Move move, Attack pattern)
        {
            Enemy instance = new Enemy((int)texture, life, this, move, pattern);
            instance.Death += HandleEnemyDeath;
            ActiveEnemies.Add(instance);
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
                        BulletAIs[(int)bullet[(int)BulletAttributes.AI]].OnDeath(
                            ref bullet[(int)BulletAttributes.CenterX], ref bullet[(int)BulletAttributes.CenterY], 
                            ref bullet[(int)BulletAttributes.DirectionX], 
                            ref bullet[(int)BulletAttributes.DirectionY], ref bullet[(int)BulletAttributes.Rotation],
                            ref bullet[(int)BulletAttributes.Speed], ref bullet[(int)BulletAttributes.Shape],
                            ref bullet[(int)BulletAttributes.SizeX], ref bullet[(int)BulletAttributes.SizeY],
                            ref bullet[(int)BulletAttributes.Time], ref bullet[(int)BulletAttributes.DrawAI],
                            ref bullet[(int)BulletAttributes.HitLayer], ref bullet[(int)BulletAttributes.Damage],
                            ref bullet[(int)BulletAttributes.Param1], ref bullet[(int)BulletAttributes.Param2],
                            ref bullet[(int)BulletAttributes.Param3], ref bullet[(int)BulletAttributes.Param4], 
                            this);
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
            switch (bullet[(int)BulletAttributes.HitLayer])
            {
                case (int)HitLayers.Player:
                    if (Collide(bullet, Player.hitbox))
                    {
                        Player.OnHit(bullet[(int)BulletAttributes.Damage]);
                        return true;
                    }
                    return false;
                case (int)HitLayers.Enemy:
                    foreach (Enemy enemy in ActiveEnemies)
                    {
                        if (Collide(bullet, enemy.hitbox))
                        {
                            enemy.OnHit(bullet[(int)BulletAttributes.Damage]);
                            return true;
                        }
                    }
                    return false;
            }
            return false;
        }

        public void HandleEnemyDeath(object sender, EnemyDeathEventArgs e)
        {
            if (e.notify)
            {
                GD.Print($"An enemy died and dropped {e.points}");
            } else
            {
                GD.Print("An enemy died, but don't tell anyone");
            }
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
