using Godot;
using PathtoDarkSide;
using PathtoDarkSide.Content;
using PathtoDarkSide.Content.Utils;
using System;

public partial class Main : Node2D
{
    public static RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
    public static BulletField bulletField = new BulletField();

    public static int defaultDifficulty;
    
    public static int gameplayTimeFlow;
    public int timeModification = 0;
    
    public static bool stoppedTime = false;

    public override void _Ready()
    {
        // Load stored default difficulty and set it
        randomNumberGenerator.Randomize();
        TexturesTable.Initialize();
        DrawEngine.Initialize();
        AddChild(DrawEngine.DrawingField);
        bulletField.Initialize(GetViewportRect());
        bulletField.ModifyTime += HandleTimeModeChange;
    }

    public override void _Process(double delta)
    {
        float fps = 0;
        if (delta > 0) fps = (float)(60f / (60f * delta));

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