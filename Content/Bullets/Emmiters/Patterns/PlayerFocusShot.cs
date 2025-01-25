using Godot;
using PathtoDarkSide.Content.Utils;

namespace PathtoDarkSide.Content.Bullets.Emmiters.Patterns
{
    public class PlayerFocusShot : Pattern
    {
        private float rotation = 0;

        public PlayerFocusShot(BulletField field) : base(field)
        {
            BulletSpawn += field.HandleBulletSpawn;
        }

        public override bool Shoot(Rect2 margin, Player player, Vector2 position, int cycle, int maxCycle, 
            Vector2 target, int dificulty, int speed, params float[] parameters)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = new Vector2(Mathf.Sin((rotation + i) * 5) * 8, 
                    Mathf.Sin((rotation + i) * 8) * 20);

                SpawnBullet(this, new BulletSpawnEventArgs(position + offset - new Vector2(10, 0), new Vector2(1, 0), 0, 20, sizeX: 30,
                     sizeY: 30, hitLayer: (int)HitLayers.Enemy, visualParam1: (int)Textures.Star, r: 0.8f, g: 0.1f,
                     b: 0.865f, a: 0.35f, ai1: 0.2f, damage: parameters[1]));
            }
            rotation += 0.01f;
            return true;
        }

        public override bool CanShoot(Vector2 position, Rect2 margin, Player player)
        {
            return true;
        }
    }
}
