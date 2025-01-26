using Godot;

namespace PathtoDarkSide.Content.Enemies.AI
{
    public class Behavior
    {
        public int type;

        public Behavior(int type)
        {
            this.type = type;
        }

        public virtual void Initialize(int speed, BulletField field, Enemy enemy)
        {
            enemy.position = new Vector2(field.Margin.Size.X + 20, field.Margin.Position.Y);
            if (type == -1)
            {
                enemy.position.Y += Main.randomNumberGenerator.RandfRange(field.Margin.Position.Y, field.Margin.Size.Y);
            }
            if (type == -2)
            {
                enemy.position.X = 600;
                enemy.position.Y = 300;
            }
        }

        public virtual void Update(int speed, Rect2 margin, Player player, Enemy enemy)
        {
            if (type == -2)
            {
                return;
            }
            enemy.position.X -= 3 + (speed*0.25f);
        }

        public bool DeathCondition(Vector2 position, BulletField field)
        {
            return position.X < field.Margin.Position.X - 20;
        }
    }
}
