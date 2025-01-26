using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathtoDarkSide.Content.Utils
{
    public enum TimeMode
    {
        Normal,
        Slow, 
        Stopped
    }

    public enum DrawTypes
    {
        Simple,
        BackAndFront, 
        Laser
    }

    public enum Effects
    {
        None,
        FadeoutShrink
    }

    public enum Textures
    {
        Player,
        Hitbox,
        Pixie,
        Outer,
        Inner,
        Star, 
        Laser
    }

    public enum HitLayers
    {
        Player,
        Enemy
    }

    public enum MoveAIs
    {
        Base
    }

    public enum PatternAIs
    {
        Circle, 
        Laser
    }
}
