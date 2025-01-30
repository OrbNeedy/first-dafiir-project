using Godot;
using System;

namespace PathtoDarkSide.Content.Enemies.AI
{
    public class Behavior
    {
        public int type;

        public event EventHandler<ModifyTimeEventArgs> ModifyTime;

        public Behavior(int type)
        {
            this.type = type;
        }

        public virtual void Initialize(int speed, Enemy enemy)
        {
            ModifyTime += Main.bulletField.HandleTimeChange;
            enemy.position = new Vector2(Main.bulletField.Margin.Size.X + 20, Main.bulletField.Margin.Position.Y);
            switch (type)
            {
                case 0:
                    break;
            }
            // Test purpose types
            if (type == -1)
            {
                enemy.position.Y += Main.randomNumberGenerator.RandfRange(Main.bulletField.Margin.Position.Y, 
                    Main.bulletField.Margin.Size.Y);
            }
            if (type == -2)
            {
                enemy.position.X = 600;
                enemy.position.Y = 300;
            }
        }

        public virtual void Update(int speed, Enemy enemy)
        {
            if (type == -2)
            {
                return;
            }
            enemy.position.X -= 3 + (speed*0.25f);
        }

        public void ChangeTimeFlow(ModifyTimeEventArgs e)
        {
            ModifyTime?.Invoke(this, e);
        }

        public bool DeathCondition(Vector2 position)
        {
            return position.X < Main.bulletField.Margin.Position.X - 20;
        }
    }
}
