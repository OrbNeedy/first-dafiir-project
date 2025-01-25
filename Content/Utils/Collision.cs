using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathtoDarkSide.Content.Utils
{
    public enum Shapes
    {
        Circle, 
        Rectangle, 
        Line
    }

    public static class Collision
    {
        // All obtained from https://www.jeffreythompson.org/collision-detection/line-line.php
        public static bool AabbvAabb(Aabb box1, Aabb box2)
        {
            return box1.Intersects(box2);
        }

        public static bool AabbvCircle(Aabb box, Vector2 circleCenter, float radius)
        {
            float distX = 0;
            float distY = 0;

            if (circleCenter.X < box.Position.X)
            {
                distX = circleCenter.X - box.Position.X;
            } else if (circleCenter.X > box.Position.X + box.Size.X)
            {
                distX = circleCenter.X - box.Position.X + box.Size.X;
            }

            if (circleCenter.Y < box.Position.Y)
            {
                distY = circleCenter.Y - box.Position.Y;
            }
            else if (circleCenter.Y > box.Position.Y + box.Size.Y)
            {
                distY = circleCenter.Y - box.Position.Y + box.Size.Y;
            }

            float distance = distX + distY;
            
            return distance <= radius;
        }

        public static bool AabbvLine(Aabb box, Vector2 lineStart, Vector2 lineEnd, float width = 1)
        {
            bool left = LinevLine(lineStart, lineEnd, new Vector2(box.Position.X, box.Position.Y), 
                new Vector2(box.Position.X, box.Position.Y + box.Size.Y), width1: width);
            bool top = LinevLine(lineStart, lineEnd, new Vector2(box.Position.X, box.Position.Y), 
                new Vector2(box.Position.X + box.Size.X, box.Position.Y), width1: width);
            bool right = LinevLine(lineStart, lineEnd, new Vector2(box.Position.X + box.Size.X, box.Position.Y), 
                new Vector2(box.Position.X + box.Size.X, box.Position.Y + box.Size.Y), width1: width);
            bool bottom = LinevLine(lineStart, lineEnd, new Vector2(box.Position.X + box.Size.X, box.Position.Y + 
                box.Size.Y), new Vector2(box.Position.X, box.Position.Y + box.Size.Y), width1: width);
            return (left || top || right || bottom);
        }

        public static bool LinevCircle(Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, float radius, 
            float width = 1)
        {
            if (width <= 0) return false;
            bool startInside = lineStart.DistanceTo(circleCenter) <= radius;
            bool endInside = lineEnd.DistanceTo(circleCenter) <= radius;
            if (startInside || endInside) return true;

            float length = (lineEnd - lineStart).LengthSquared();
            float dot = (((circleCenter.X - lineStart.X) * (lineEnd.X - lineStart.X)) + 
                ((circleCenter.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y))) / Mathf.Pow(length, 2);
            Vector2 closestPoint = lineStart + 
                new Vector2(dot * (lineEnd.X - lineStart.X), dot * (lineEnd.Y - lineStart.Y));

            return closestPoint.DistanceTo(circleCenter) <= radius+width;
        }

        public static bool LinevLine(Vector2 line1Start, Vector2 line1End, Vector2 line2Start,
            Vector2 line2End, float width1 = 1, float width2 = 1)
        {
            if (width1 <= 0 || width2 <= 0) return false;
            float x1 = line1Start.X;
            float y1 = line1Start.Y;
            float x2 = line1End.X;
            float y2 = line1End.Y;
            float x3 = line2Start.X;
            float y3 = line2Start.Y;
            float x4 = line2End.X;
            float y4 = line2End.Y;

            // I can't understand the logic behind this
            // Paul Bourke wrote about this exact algorithm in here 
            // https://web.archive.org/web/20060911055655/http://local.wasp.uwa.edu.au/%7Epbourke/geometry/lineline2d/
            // And Jeffrey Thompson used it to make a Line on Line collision function, which is this one 
            // But I still can't understand it
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) /
                ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) /
                ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            return (uA >= 0 && uA <= width1 && uB >= 0 && uB <= width2);
        }

        public static bool CirclevCircle(Vector2 circleCenter1, float radius1, Vector2 circleCenter2, float radius2)
        {
            return circleCenter1.DistanceTo(circleCenter2) <= radius1 + radius2;
        }
    }
}
