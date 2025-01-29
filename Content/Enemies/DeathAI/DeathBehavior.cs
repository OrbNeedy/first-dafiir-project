using System;
using Godot;

namespace PathtoDarkSide.Content.Enemies.DeathAI
{
    public class DeathBehavior
    {
        public int type;

        public event EventHandler<ModifyTimeEventArgs> ModifyTime;
        public event EventHandler<BulletSpawnEventArgs> BulletSpawn;

        public DeathBehavior(int type)
        {
            this.type = type;
        }

        public virtual void Initialize(Enemy enemy, BulletField field)
        {
            ModifyTime += field.HandleTimeChange;
            BulletSpawn += field.HandleBulletSpawn;
        }

        public virtual void DeathOccured(Enemy enemy)
        {

        }

        public void ChangeTimeFlow(ModifyTimeEventArgs e)
        {
            ModifyTime?.Invoke(this, e);
        }

        protected void SpawnBullet(BulletSpawnEventArgs e)
        {
            BulletSpawn?.Invoke(this, e);
        }
    }
}
