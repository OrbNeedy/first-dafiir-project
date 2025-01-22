using Godot;
using PathtoDarkSide;
using PathtoDarkSide.Content;
using PathtoDarkSide.Content.Utils;

public partial class Main : Node2D
{
    public BulletField bulletField;
    public RandomNumberGenerator randomNumberGenerator;
    public Player player;

    public override void _Ready()
    {
        TexturesTable.Initialize();
        DrawEngine.Initialize();
        bulletField = new BulletField(GetViewportRect());
        randomNumberGenerator = new RandomNumberGenerator();
        randomNumberGenerator.Randomize();
        player = new Player(bulletField);
        AddChild(DrawEngine.DrawingField);
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

        player.HandleInputs();
        player.Update(delta, bulletField.Margin);
        player.Draw();
    }
}
