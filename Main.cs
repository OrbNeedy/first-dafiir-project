using Godot;
using PathtoDarkSide;
using PathtoDarkSide.Content;
using PathtoDarkSide.Content.Utils;
using System;

public partial class Main : Node2D
{
    public static RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
    public ulong seed = 0;
    public static BulletField bulletField = new BulletField();

    public static int defaultDifficulty;
    
    public static int gameplayTimeFlow;
    public int timeModification = 0;
    
    public static bool stoppedTime = false;

    public override void _Ready()
    {
        // Load stored default difficulty and set it
        randomNumberGenerator.Randomize();
        seed = randomNumberGenerator.Seed;
        TexturesTable.Initialize();
        DrawEngine.Initialize();
        bulletField.Initialize(GetViewportRect());
        AddChild(DrawEngine.DrawingField);
        bulletField.ModifyTime += HandleTimeModeChange;
    }

    public override void _Process(double delta)
    {
        float fps = 0;
        if (delta > 0) fps = (float)(60f / (60f * delta));

        /*if (randomNumberGenerator.Randf() <= 0.25f)
        {
            for (int i = 0; i < 2; i++)
            {
                bulletField.AddProjectile(new Vector2(600, 300), 
                    Vector2.Right.Rotated(randomNumberGenerator.RandfRange(0, Mathf.Tau)), 0, 
                    randomNumberGenerator.RandfRange(1, 3), drawScript: (int)DrawTypes.BackAndFront, 
                    visualParam1: (int)Textures.Inner, visualParam2: (int)Textures.Outer, g: 0, b: 0);
            }
        }*/

        ProcessInputs();
        bulletField.Update(delta, stoppedTime);
    }

    private void ProcessInputs()
    {
        if (Input.IsActionJustPressed("space"))
        {
            timeModification++;
            if (timeModification > 2) timeModification = 0;
        }

        switch (timeModification)
        {
            case 0:
                stoppedTime = false;
                break;
            case 1:
                stoppedTime = !stoppedTime;
                break;
            case 2:
                stoppedTime = true;
                break;
        }
    }

    public void HandleTimeModeChange(object? sender, ModifyTimeEventArgs e)
    {
        timeModification = e.newTime;
    }
}

namespace PathtoDarkSide
{
    public class ModifyTimeEventArgs : EventArgs
    {
        public int newTime;

        public ModifyTimeEventArgs(int newTime)
        {
            this.newTime = newTime;
            if (newTime > (int)TimeMode.Stopped) this.newTime = (int)TimeMode.Stopped;
            if (newTime < (int)TimeMode.Normal) this.newTime = (int)TimeMode.Normal;
        }
    }
}