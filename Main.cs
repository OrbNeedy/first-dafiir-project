using Godot;
using PathtoDarkSide.Content;
using PathtoDarkSide.Content.Enemies;
using PathtoDarkSide.Content.Utils;
using System.Collections.Generic;
using PathtoDarkSide.Content.Bullets.Emmiters;
using PathtoDarkSide.Content.Bullets.Emmiters.Patterns;


public partial class Main : Node2D
{
    public BulletField bulletField;
    public RandomNumberGenerator randomNumberGenerator;
    public List<Enemy> enemies = new List<Enemy>();
    public Player player;

    public override void _Ready()
    {
        TexturesTable.Initialize();
        DrawEngine.Initialize();
        AddChild(DrawEngine.DrawingField);
        bulletField = new BulletField(GetViewportRect());
        bulletField.main = this;
        randomNumberGenerator = new RandomNumberGenerator();
        randomNumberGenerator.Randomize();
        player = new Player(bulletField);
        // TODO: Change the way new Emitters are added to enemies
        enemies.Add(new Enemy(new Vector2(600, 100), (int)Textures.Pixie, 300, new Emitter[] { 
            new Emitter(new Vector2(600, 100), new Pattern(), 120, 2) }, bulletField));
        enemies[0].Death += HandleEnemyDeath;
    }

    public override void _Process(double delta)
    {
        float fps = 0;
        if (delta > 0) fps = (float)(60f / (60f * delta));

        if (randomNumberGenerator.RandiRange(0, 100) < 50)
        {
            bulletField.AddProjectile(new Vector2(600, 300),
                new Vector2(1, 0).Rotated(randomNumberGenerator.RandfRange(0, MathConsts.TwoPi)), 0,
                randomNumberGenerator.RandfRange(0.5f, 5), drawScript: (int)DrawTypes.BackAndFront,
                visualParam1: (int)Textures.Inner, visualParam2: (int)Textures.Outer, r: 1, g: 0.1f, b: 0.1f);
        }

        bulletField.Update(delta);

        foreach (Enemy enemy in enemies)
        {
            enemy.Update();
            enemy.Draw();
        }

        player.HandleInputs();
        player.Update(delta, bulletField.Margin);
        player.Draw();
    }

    public void HandleEnemyDeath(object sender, EnemyDeathEventArgs e)
    {
        GD.Print($"Enemy died and droped {e.points} points.");
        enemies.Remove((Enemy)sender);
    }
}
