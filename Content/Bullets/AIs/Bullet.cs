using Godot;

namespace PathtoDarkSide.Content.Bullets.Emmiters
{
    public class Bullet
    {
        public virtual void UpdateBullet(ref float centerX, ref float centerY, ref float directionX, 
            ref float directionY, ref float rotation, ref float speed, ref float shape, ref float sizeX, 
            ref float sizeY, ref float time, ref float drawScript, ref float layer, ref float damage,
            ref float ai1, ref float ai2, ref float ai3, ref float ai4, BulletField field)
        {
            centerX += directionX * speed;
            centerY += directionY * speed;

            rotation += ai1;
        }

        public virtual void OnDeath(ref float centerX, ref float centerY, ref float directionX, ref float directionY,
            ref float rotation, ref float speed, ref float shape, ref float sizeX, ref float sizeY, ref float time,
            ref float drawScript, ref float layer, ref float damage, ref float ai1, ref float ai2, ref float ai3, 
            ref float ai4, BulletField field)
        {

        }
    }
}