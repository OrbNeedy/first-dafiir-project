﻿using Godot;
using PathtoDarkSide.Content.Utils;
using static PathtoDarkSide.Content.Utils.VisualTables;
using System.Collections.Generic;
using PathtoDarkSide.Content;
using System.Collections.Specialized;


public partial class DrawingField : Control
{
    // TODO: Drawing strings
    public static List<float[]> DrawQueue = new List<float[]>();

    public override void _Ready()
    {
        TextureFilter = TextureFilterEnum.Linear;
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        Vector2 spritePosition = new Vector2();
        Vector2 spriteOffset = new Vector2();
        Vector2 scale = new Vector2();
        float rotation = 0;
        Color modulate = new Color();
        int frame = 0;

        //DrawQueue.Sort(CompareByLayer);

        foreach (var drawInstruction in DrawQueue)
        {
            if (drawInstruction == null) continue;
            spritePosition = Vector2.Zero;
            spriteOffset = Vector2.Zero;
            scale = Vector2.One;
            rotation = 0;
            modulate.R = 1;
            modulate.G = 1;
            modulate.B = 1;
            modulate.A = 1;
            frame = (int)drawInstruction[(int)EffectAttributes.Frame];

            if (frame >= TexturesTable.LoadedTextures[(int)drawInstruction[(int)EffectAttributes.Texture]].Length)
            {
                frame = TexturesTable.LoadedTextures[(int)drawInstruction[(int)EffectAttributes.Texture]].Length - 1;
            }
            if (frame < 0) frame = 0;

            spritePosition.X = drawInstruction[(int)EffectAttributes.PositionX];
            spritePosition.Y = drawInstruction[(int)EffectAttributes.PositionY];

            spriteOffset = TexturesTable.LoadedTextures[(int)drawInstruction[(int)EffectAttributes.Texture]]
                [frame].GetSize() / 2;

            modulate.R = drawInstruction[(int)EffectAttributes.R];
            modulate.G = drawInstruction[(int)EffectAttributes.G];
            modulate.B = drawInstruction[(int)EffectAttributes.B];
            modulate.A = drawInstruction[(int)EffectAttributes.A];

            rotation = drawInstruction[(int)EffectAttributes.Rotation];
            scale.X = drawInstruction[(int)EffectAttributes.ScaleX];
            scale.Y = drawInstruction[(int)EffectAttributes.ScaleY];

            DrawSetTransform(spritePosition, rotation, scale);
            DrawTexture(TexturesTable.LoadedTextures[(int)drawInstruction[(int)EffectAttributes.Texture]]
                [frame], -spriteOffset, modulate);

            if (!Main.stoppedTime)
            {
                drawInstruction[(int)EffectAttributes.Time] -= 1;

                VisualEffects[(int)drawInstruction[(int)EffectAttributes.AI]].Update(
                ref drawInstruction[(int)EffectAttributes.Texture],
                ref drawInstruction[(int)EffectAttributes.PositionX],
                ref drawInstruction[(int)EffectAttributes.PositionY],
                ref drawInstruction[(int)EffectAttributes.Rotation],
                ref drawInstruction[(int)EffectAttributes.ScaleX], ref drawInstruction[(int)EffectAttributes.ScaleY],
                ref drawInstruction[(int)EffectAttributes.R], ref drawInstruction[(int)EffectAttributes.G],
                ref drawInstruction[(int)EffectAttributes.B], ref drawInstruction[(int)EffectAttributes.A],
                ref drawInstruction[(int)EffectAttributes.Layer], ref drawInstruction[(int)EffectAttributes.Frame],
                ref drawInstruction[(int)EffectAttributes.Time], ref drawInstruction[(int)EffectAttributes.Param1],
                ref drawInstruction[(int)EffectAttributes.Param2]);
            }
        }

        DrawQueue.RemoveAll((x) => x[(int)EffectAttributes.Time] <= 0);
    }

    private int CompareByLayer(float[] instruction1, float[] instruction2)
    {
        //(x, y) => (int)x[(int)EffectAttributes.Layer] - (int)y[(int)EffectAttributes.Layer];
        if (instruction1[(int)EffectAttributes.Layer] < instruction2[(int)EffectAttributes.Layer])
        {
            return -1;
        }
        if (instruction1[(int)EffectAttributes.Layer] > instruction2[(int)EffectAttributes.Layer])
        {
            return 1;
        }
        return 0;
    }
}
