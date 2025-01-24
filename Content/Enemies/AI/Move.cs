using Godot;
using PathtoDarkSide.Content.Bullets.Emmiters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathtoDarkSide.Content.Enemies.AI
{
    public class Move
    {
        public int type;

        public Move(int type)
        {
            this.type = type;
        }

        public virtual void Initialize(int speed, BulletField field, ref Vector2 position)
        {
            position = new Vector2(field.Margin.Size.X + 20, field.Margin.Position.Y);
            if (type == -1)
            {
                position.Y += Main.randomNumberGenerator.RandfRange(field.Margin.Position.Y, field.Margin.Size.Y);
            }
        }

        public virtual void Update(int speed, Rect2 margin, Player player, ref Vector2 position)
        {
            position.X -= 3 + (speed*0.25f);
        }

        public bool DeathCondition(Vector2 position, BulletField field)
        {
            return position.X < field.Margin.Position.X - 20;
        }
    }
}
