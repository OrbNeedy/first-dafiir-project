using Godot;
using PathtoDarkSide.Content.Utils;
using System.Collections.Generic;

namespace PathtoDarkSide.Content.Stages
{
    // Test stages will be very long files
    [GlobalClass]
    public partial class TestStage : Resource
    {
        public Dictionary<int, List<float[]>> EnemyDictionary { get; set; }
        public Dictionary<int, List<float[]>> BulletDictionary { get; set; }

        public TestStage()
        {
            EnemyDictionary = new Dictionary<int, List<float[]>>();
            EnemyDictionary[600] = new List<float[]> { 
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
            };
            EnemyDictionary[630] = new List<float[]> {
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
            };
            EnemyDictionary[660] = new List<float[]> {
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
            };
            EnemyDictionary[1800] = new List<float[]> {
                new[] { (float)Textures.Pixie, 300f, 0f, -2f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 0f }
            };
            EnemyDictionary[1920] = new List<float[]> {
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
            };
            EnemyDictionary[3000] = new List<float[]> {
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
            };
            EnemyDictionary[3600] = new List<float[]> {
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
            };
            EnemyDictionary[6000] = new List<float[]> {
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
                new[] { (float)Textures.Pixie, 30f, 0f, -1f, 0f, 0f, 50f, 50f, 25f, 25f, 0f, 0f, 18f, 0f, 1f },
            };
        }
    }
}
