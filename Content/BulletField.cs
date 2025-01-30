using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Enemies.AI;
using PathtoDarkSide.Content.Enemies.OffensiveAI;
using PathtoDarkSide.Content.Utils;
using static PathtoDarkSide.Content.Utils.VisualTables;
using System;
using System.Collections.Generic;
using PathtoDarkSide.Content.Enemies;
using PathtoDarkSide.Content.Bullets.AIs;
using PathtoDarkSide.Content.Stages;
using PathtoDarkSide.Content.Enemies.DeathAI;

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
    Width,
    Penetration,
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

public enum BulletTypes
{
    Simple, 
    Laser
}

namespace PathtoDarkSide.Content
{
    public class BulletSpawnEventArgs : EventArgs
    {
        public Vector2 position;
        public Vector2 direction;
        public float rotation;
        public float speed;
        public float shape;
        public float sizeX;
        public float sizeY;
        public float width;
        public float penetration;
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
            float shape = 0, float sizeX = 10, float sizeY = 10, float width = 1, float penetration = 1, 
            float script = 0, float drawScript = 0, float hitLayer = (int)HitLayers.Player, float damage = 1, 
            float ai1 = 0, float ai2 = 0, float ai3 = 0, float ai4 = 0, float visualParam1 = 0, 
            float visualParam2 = 0, float r = 1, float g = 1, float b = 1, float a = 1)
        {
            this.position = position;
            this.direction = direction;
            this.rotation = rotation;
            this.speed = speed;
            this.shape = shape;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.width = width;
            this.penetration = penetration;
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

    public class SpawnEnemyEventArgs : EventArgs
    {
        public Textures texture;
        public float life;
        public Behavior behavior;
        public AttackBehavior attackBehavior;
        public DeathBehavior? deathBehavior;
        public Vector2 hitboxSize;
        public Vector2 hurtboxSize;
        public int maxFrameCount;
        public int shape;
        public bool sealSpawns;

        public SpawnEnemyEventArgs(Textures texture, float life, Behavior behavior, AttackBehavior attackBehavior,
            Vector2 hitboxSize, Vector2 hurtboxSize, DeathBehavior? deathBehavior = null, int maxFrameCount = 20, 
            int shape = (int)Shapes.Circle, bool sealSpawns = false)
        {
            this.texture = texture;
            this.life = life;
            this.behavior = behavior;
            this.attackBehavior = attackBehavior;
            this.deathBehavior = deathBehavior;
            this.hitboxSize = hitboxSize;
            this.hurtboxSize = hurtboxSize;
            this.maxFrameCount = maxFrameCount;
            this.shape = shape;
            this.sealSpawns = sealSpawns;
        }
    }

    public class NotifyEventArgs : EventArgs
    {
        public int value;
        public NotificationType type;

        public NotifyEventArgs(int value, NotificationType type)
        {
            this.value = value;
            this.type = type;
        }
    }

    public class DenySpawnEventArgs : EventArgs
    {
        public Enemy source;

        public DenySpawnEventArgs(Enemy source)
        {
            this.source = source;
        }
    }

    public class BulletField
    {
        public Rect2 Margin;
        public static Bullet[] BulletAIs = new Bullet[] { new Bullet(), new Laser() };
        public List<float[]> ActiveBullets = new List<float[]>();
        public List<Enemy> ActiveEnemies = new List<Enemy>();
        public Player Player;
        public Stage Stage;
        
        public bool hidePlayer = false;
        public bool paused = false;
        public Enemy? sealEnemy;
        public bool allowSpawn = true;

        public event EventHandler<ModifyTimeEventArgs> ModifyTime;

        public BulletField()
        {

        }

        public void Initialize(Rect2 margin)
        {
            Margin = margin;
            Player = new Player();
            Stage = new Stage();
            Stage.Initialize();
            //AddEnemy(Textures.Pixie, 5000, new Behavior(-2), new ConcentratedLaser(0));
        }

        public void Update(double delta, bool stoppedTime)
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
                        ref bullet[(int)BulletAttributes.Width], ref bullet[(int)BulletAttributes.Time], 
                        ref bullet[(int)BulletAttributes.DrawAI], ref bullet[(int)BulletAttributes.HitLayer], 
                        ref bullet[(int)BulletAttributes.Damage], ref bullet[(int)BulletAttributes.Param1], 
                        ref bullet[(int)BulletAttributes.Param2], ref bullet[(int)BulletAttributes.Param3], 
                        ref bullet[(int)BulletAttributes.Param4]);
                }

                // Postdraw bullet
                DrawingAIs[(int)bullet[(int)BulletAttributes.DrawAI]].Postdraw(bullet[(int)BulletAttributes.CenterX],
                    bullet[(int)BulletAttributes.CenterY], bullet[(int)BulletAttributes.DirectionX],
                    bullet[(int)BulletAttributes.DirectionY], bullet[(int)BulletAttributes.SizeX],
                    bullet[(int)BulletAttributes.SizeY], bullet[(int)BulletAttributes.Rotation],
                    bullet[(int)BulletAttributes.Time], ref bullet[(int)BulletAttributes.VisualParam1], 
                    ref bullet[(int)BulletAttributes.VisualParam2], bullet[(int)BulletAttributes.R], 
                    bullet[(int)BulletAttributes.G], bullet[(int)BulletAttributes.B], 
                    bullet[(int)BulletAttributes.A]);

                if (!stoppedTime && !paused)
                {
                    bullet[(int)BulletAttributes.Time] += 1;
                    BulletCollisionCheck(bullet);
                }
            }

            ActiveBullets.RemoveAll(CheckOutOfBounds);

            foreach (Enemy enemy in ActiveEnemies)
            {
                if ((!stoppedTime || enemy.immuneToStopTime) && !paused)
                {
                    enemy.Update(Main.defaultDifficulty, Stage.stageSpeed);
                    if (Player.iFrames <= 0)
                    {
                        if (Player.Collided(enemy))
                        {
                            Player.OnHit(enemy.damageValue);
                        }
                    }
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

            if (!stoppedTime && !paused)
            {
                Stage.Update();
            }

            if (Main.randomNumberGenerator.RandiRange(0, 100) < -10 && !stoppedTime && !paused)
            {
                AddEnemy(Textures.Pixie, 40, new Behavior(-1), new AttackBehavior(0));
            }
        }

        public void HandleBulletSpawn(object sender, BulletSpawnEventArgs e)
        {
            AddProjectile(e.position, e.direction, e.rotation, e.speed, e.shape, e.sizeX, e.sizeY, e.width, 
                e.penetration, e.script, e.drawScript, e.hitLayer, e.damage, e.ai1, e.ai2, e.ai3, e.ai4, 
                e.visualParam1, e.visualParam2, e.r, e.g, e.b, e.a);
        }

        public void AddProjectile(Vector2 position, Vector2 direction, float rotation, float speed, float shape = 0,
            float sizeX = 10, float sizeY = 10, float width = 1, float penetration = 1, float script = 0, 
            float drawScript = 0, float hitLayer = (int)HitLayers.Player, float damage = 1, float ai1 = 0, 
            float ai2 = 0, float ai3 = 0, float ai4 = 0, float visualParam1 = 0, float visualParam2 = 0, 
            float r = 1, float g = 1, float b = 1, float a = 1)
        {
            direction = direction.Normalized();

            float[] bullet = new float[] { position.X, position.Y, direction.X, direction.Y, rotation, speed, shape,
                sizeX, sizeY, width, penetration, 0, script, drawScript, hitLayer, damage, ai1, ai2, ai3, ai4, 
                visualParam1, visualParam2, r, g, b, a };

            ActiveBullets.Add(bullet);
        }

        public void HandleEnemySpawn(object sender, SpawnEnemyEventArgs e)
        {
            GD.Print("Registered spawn seal enemy: " + sealEnemy);
            if (allowSpawn && sealEnemy == null)
            {
                AddEnemy(e.texture, e.life, e.behavior, e.attackBehavior, e.hitboxSize, e.hurtboxSize, 
                    e.deathBehavior, e.maxFrameCount, e.shape, e.sealSpawns);
            }
        }

        public void AddEnemy(Textures texture, float life, Behavior behavior, AttackBehavior attackBehavior, 
            Vector2 hitboxSize, Vector2 hurtboxSize, DeathBehavior? deathBehavior = null, int maxFrameCount = 18, 
            int shape = (int)Shapes.Circle, bool sealSpawns = false)
        {
            Enemy instance = new Enemy((int)texture, life, behavior, deathBehavior, attackBehavior, hitboxSize, 
                hurtboxSize, maxFrameCount, shape);
            instance.Death += HandleEnemyDeath;
            ActiveEnemies.Add(instance);
            if (sealSpawns)
            {
                sealEnemy = instance;
            }
        }

        public void AddEnemy(Textures texture, float life, Behavior move, AttackBehavior pattern, 
            int maxFrameCount = 18)
        {
            Enemy instance = new Enemy((int)texture, life, move, null, pattern, Vector2.One * 50, 
                Vector2.One * 25, maxFrameCount, (int)Shapes.Circle);
            instance.Death += HandleEnemyDeath;
            ActiveEnemies.Add(instance);
        }

        public void ClearScreen()
        {
            ActiveBullets.Clear();
        }

        public void BulletCollisionCheck(float[] bullet)
        {
            // If the bullet is out of bounds, delete it
            if (CheckOutOfBounds(bullet)) return;

            bool hit = false;
            // Check what layer the bullet targets so it doesn't hit anything unintentionally 
            switch (bullet[(int)BulletAttributes.HitLayer])
            {
                case (int)HitLayers.Player:
                    // The player's hitbox is always a circle, the collision depends completely on the bullet's shape
                    if (Player.Collided(bullet))
                    {
                        //GD.Print("Player got hit");
                        Player.OnHit(bullet[(int)BulletAttributes.Damage]);
                        bullet[(int)BulletAttributes.Penetration]--;
                        hit = true; 
                        break;
                    }
                    break;
                case (int)HitLayers.Enemy:
                    // One check for each enemy
                    foreach (Enemy enemy in ActiveEnemies)
                    {
                        if (enemy.Collided(bullet))
                        {
                            //GD.Print("Enemy got hit");
                            enemy.OnHit(bullet[(int)BulletAttributes.Damage], Margin);
                            bullet[(int)BulletAttributes.Penetration]--;
                            hit = true;
                            break;
                        }
                    }
                    break;
            }

            if (hit && bullet[(int)BulletAttributes.Penetration] == 0)
            {
                BulletAIs[(int)bullet[(int)BulletAttributes.AI]].OnDeath(
                    ref bullet[(int)BulletAttributes.CenterX], ref bullet[(int)BulletAttributes.CenterY],
                    ref bullet[(int)BulletAttributes.DirectionX],
                    ref bullet[(int)BulletAttributes.DirectionY], ref bullet[(int)BulletAttributes.Rotation],
                    ref bullet[(int)BulletAttributes.Speed], ref bullet[(int)BulletAttributes.Shape],
                    ref bullet[(int)BulletAttributes.SizeX], ref bullet[(int)BulletAttributes.SizeY],
                    ref bullet[(int)BulletAttributes.Width], ref bullet[(int)BulletAttributes.Time], 
                    ref bullet[(int)BulletAttributes.DrawAI], ref bullet[(int)BulletAttributes.HitLayer], 
                    ref bullet[(int)BulletAttributes.Damage], ref bullet[(int)BulletAttributes.Param1], 
                    ref bullet[(int)BulletAttributes.Param2], ref bullet[(int)BulletAttributes.Param3], 
                    ref bullet[(int)BulletAttributes.Param4]);
            }
        }

        public void HandleEnemyDeath(object sender, EnemyDeathEventArgs e)
        {
            // Add score
            if (sealEnemy == sender)
            {
                sealEnemy = null;
            }
        }

        public void HandleTimeChange(object sender, ModifyTimeEventArgs e)
        {
            ModifyTime?.Invoke(sender, e);
        }

        public bool CheckOutOfBounds(float[] bullet)
        {
            int limit = 100;
            bool outOfPenetration = bullet[(int)BulletAttributes.Penetration] == 0f;
            return bullet[(int)BulletAttributes.CenterX] >= Margin.Position.X + Margin.Size.X + limit || 
                bullet[(int)BulletAttributes.CenterX] <= Margin.Position.X - limit ||
                bullet[(int)BulletAttributes.CenterY] >= Margin.Position.Y + Margin.Size.Y + limit || 
                bullet[(int)BulletAttributes.CenterY] <= Margin.Position.Y - limit || 
                outOfPenetration;
        }

        public int GetBulletCount()
        {
            return ActiveBullets.Count;
        }
    }
}