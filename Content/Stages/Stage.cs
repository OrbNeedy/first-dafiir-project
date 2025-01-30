using Godot;
using PathtoDarkSide.Content.Enemies.AI;
using PathtoDarkSide.Content.Enemies.DeathAI;
using PathtoDarkSide.Content.Enemies.OffensiveAI;
using PathtoDarkSide.Content.Utils;
using System;
using System.Linq;

namespace PathtoDarkSide.Content.Stages
{
    public class Stage
    {
        private TestStage stageLayout = new TestStage();
        public int stageTime = 0;
        public int stageSpeed = 0;

        public event EventHandler<BulletSpawnEventArgs> BulletSpawn;
        public event EventHandler<SpawnEnemyEventArgs> EnemySpawn;

        public void Initialize()
        {
            BulletSpawn += Main.bulletField.HandleBulletSpawn;
            EnemySpawn += Main.bulletField.HandleEnemySpawn;
        }

        public void Update()
        {
            if (stageLayout.EnemyDictionary.Keys.Contains(stageTime))
            {
                foreach (var thing in stageLayout.EnemyDictionary[stageTime])
                {
                    SpawnEnemy(new SpawnEnemyEventArgs(GetTexture(thing[0]), thing[1], 
                        GetBehavior(thing[2], thing[3]), GetAttackBehavior(thing[4], thing[5]), 
                        new Vector2(thing[6], thing[7]), new Vector2(thing[8], thing[9]), 
                        GetDeathBehavior(thing[10], thing[11]), (int)thing[12], (int)thing[13], thing[14] == 0f));
                }
            }
            stageTime++;
        }

        public Textures GetTexture(float index)
        {
            switch (index)
            {
                default:
                    return Textures.Pixie;
            }
        }

        public Behavior GetBehavior(float index, float type)
        {
            switch (index)
            {
                default:
                    return new Behavior((int)type);
            }
        }

        public AttackBehavior GetAttackBehavior(float index, float type)
        {
            switch (index)
            {
                default:
                    return new AttackBehavior((int)type);
            }
        }

        public DeathBehavior GetDeathBehavior(float index, float type)
        {
            switch (index)
            {
                default:
                    return new DeathBehavior((int)type);
            }
        }

        public void LoadStage(int location)
        {
            // Select a stage at random from a pool depending on the location
            stageTime = 0;
            stageSpeed = 0;
        }

        public void SpawnBullet(BulletSpawnEventArgs e)
        {
            BulletSpawn?.Invoke(this, e);
        }

        public void SpawnEnemy(SpawnEnemyEventArgs e)
        {
            EnemySpawn?.Invoke(this, e);
        }
    }
}
