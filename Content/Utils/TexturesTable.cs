using Godot;
using System.Collections.Generic;

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
            [(int)Textures.Hitbox] = new[] { (Texture2D)GD.Load("res://Assets/Player/Hitbox.png") },
            [(int)Textures.Outer] = new[] { (Texture2D)GD.Load("res://Assets/Bullets/OuterBullet.png") },
            [(int)Textures.Inner] = new[] { (Texture2D)GD.Load("res://Assets/Bullets/InnerBullet.png") },
            [(int)Textures.Star] = new[] { (Texture2D)GD.Load("res://Assets/Bullets/Star.png") }
        };

        public static void Initialize()
        {
            // TODO: Make the rest of the laser parts (Start and end(Optional))
            Texture2D[] laser1 = new Texture2D[6];
            for (int i = 0; i < 6; i++)
            {
                AtlasTexture texture = new AtlasTexture();
                texture.Atlas = (Texture2D)GD.Load("res://Assets/Bullets/LaserSegment.png");
                texture.Region = new Rect2(24 * i, 0, 24, 20);
                laser1[i] = texture;
            }
            LoadedTextures[(int)Textures.Laser] = laser1;

            Texture2D[] enemy1 = new Texture2D[3];
            for (int i = 0; i < 3; i++)
            {
                AtlasTexture texture = new AtlasTexture();
                texture.Atlas = (Texture2D)GD.Load("res://Assets/Enemies/LunarPixie.png");
                texture.Region = new Rect2(38 * i, 0, 38, 32);
                enemy1[i] = texture;
            }
            LoadedTextures[(int)Textures.Pixie] = enemy1;
        }
    }
}
