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

            DrawingField.DrawQueue.Add(data);
        }
    }
}
