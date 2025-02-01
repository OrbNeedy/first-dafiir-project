using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathtoDarkSide.Content.Utils
{
    public static class DrawEngine
    {
        public static DrawingField DrawingField { get; set; }

        public static void Initialize()
        {
            DrawingField = new DrawingField();
        }

        public static void AddDraw(int texture, float positionX, float positionY, float rotation,
            float scaleX = 1, float scaleY = 1, float r = 1, float g = 1, float b = 1, float a = 1, int layer = 0, 
            int frame = 0, int ai = 0, int time = 0, float ai1 = 0, float ai2 = 0)
        {
            float[] data = new float[] { texture, positionX, positionY, rotation, scaleX, scaleY, r, g, b, a, layer, 
                frame, ai, time, ai1, ai2 };

            //DrawingField.DrawQueue.Add(data);
            if (DrawingField.DrawQueue.Count <= 0)
            {
                DrawingField.DrawQueue.Add(data);
                return;
            }

            if (DrawingField.DrawQueue[DrawingField.DrawQueue.Count - 1][(int)EffectAttributes.Layer] <= layer)
            {
                DrawingField.DrawQueue.Add(data);
                return;
            }

            if (DrawingField.DrawQueue[0][(int)EffectAttributes.Layer] >= layer)
            {
                DrawingField.DrawQueue.Insert(0, data);
                return;
            }

            int index = -1;
            for (int i = 0; i < DrawingField.DrawQueue.Count; i++)
            {
                float dataLayer = DrawingField.DrawQueue[i][(int)EffectAttributes.Layer];
                if (dataLayer == layer)
                {
                    index = i - 1;
                    break;
                }
            }
            if (index < 0)
            {
                index = 0;
            }
            DrawingField.DrawQueue.Insert(index, data);
        }
    }

    public class CompareByLayer : IComparer<float[]>
    {
        public int Compare(float[] x, float[] y)
        {
            if (x[(int)EffectAttributes.Layer] < y[(int)EffectAttributes.Layer])
            {
                return -1;
            }
            if (x[(int)EffectAttributes.Layer] > y[(int)EffectAttributes.Layer])
            {
                return 1;
            }
            return 0;
        }
    }
}
