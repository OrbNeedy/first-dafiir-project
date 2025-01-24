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
    public static RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

    public override void _Ready()
    {
        TexturesTable.Initialize();
        DrawEngine.Initialize();
        AddChild(DrawEngine.DrawingField);
        bulletField = new BulletField(GetViewportRect(), this);
        randomNumberGenerator.Randomize();
        // TODO: Change the way new Emitters are added to enemies
    }

    public override void _Process(double delta)
    {
        float fps = 0;
        if (delta > 0) fps = (float)(60f / (60f * delta));

        /*if (randomNumberGenerator.RandiRange(0, 100) < 50)
        {
            bulletField.AddProjectile(new Vector2(600, 300),
                new Vector2(1, 0).Rotated(randomNumberGenerator.RandfRange(0, MathConsts.TwoPi)), 0,
                randomNumberGenerator.RandfRange(0.5f, 5), drawScript: (int)DrawTypes.BackAndFront,
                visualParam1: (int)Textures.Inner, visualParam2: (int)Textures.Outer, r: 1, g: 0.1f, b: 0.1f);
        }*/

        bulletField.Update(delta);
    }
}
