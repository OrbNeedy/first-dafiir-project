using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathtoDarkSide.Content.Utils
{
    public static class TexturesTable
    {
        // If a file is a spritesheet of more than one sprite, an atlas will be used to save a different texture2D 
        // from the same image, this means the index of the array will correspond to a different frame of the same 
        // animation
        public static Dictionary<int, Texture2D[]> LoadedTextures = new Dictionary<int, Texture2D[]>
        {
            [(int)Textures.Player] = new[] { (Texture2D)GD.Load("res://Assets/Player/TestShipHorizontal.png") },
            [(int)Textures.Outer] = new[] { (Texture2D)GD.Load("res://Assets/Bullets/OuterBullet.png") },
            [(int)Textures.Inner] = new[] { (Texture2D)GD.Load("res://Assets/Bullets/InnerBullet.png") },
            [(int)Textures.Star] = new[] { (Texture2D)GD.Load("res://Assets/Bullets/Star.png") } 
        };

        public static void Initialize()
        {
        }
    }
}
