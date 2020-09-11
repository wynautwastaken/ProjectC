using System;
using ProjectC.Engine;
using ProjectC.Engine.Objects;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProjectC.World
{
    public class WorldGenerator
    {
        public static int Seed = 2;

        public WorldGenerator()
        {
            for (var i = 0; i < 10; i++)
            {
                var chunk = ChunkedWorld.LoadChunk(new ChunkIdentifier(i, 0));
                GenerateChunk(chunk);
            }
        }

        public static void GenerateChunk(Chunk chunk)
        {
            var k = chunk.ChunkToWorld(Vector2.Zero).X / 8;
            chunk.seed = k;
            for (var i = 0; i < Chunk.ChunkWidth; i++)
            {
                var n = GenNoise(k/2, 255f);
                for (var j = Chunk.ChunkHeight; j > n; j--)
                {
                    var pos = new Vector2(i, j);
                    new Tile(EnumTiles.Fresh, chunk, pos);
                }
                k++;
            }
        }

        public static int GenNoise(float xx, float range)
        {
            xx /= 16;
            float noise = 0;

            float chunkSize = 32;

            range = range / 2;

            while(chunkSize > 0){
                float chunkIndex = xx / chunkSize;
    
                float prog = (xx % chunkSize) / chunkSize;
    
                float leftRandom = RandomSeed(chunkIndex, range);
                float rightRandom = RandomSeed(chunkIndex + 1, range);
    
                noise += (1-prog)*leftRandom + prog*rightRandom;
    
                chunkSize /= 2;
                chunkSize = (float)Math.Round(chunkSize); 
                range /= 2;
                range = Math.Max(1,(float)Math.Round(range));
            }

            return (int)noise;
        }

        public static float RandomSeed(float index, float range)
        {
            var rand = new Random(Seed * (int) index);
            return (float)rand.NextDouble() * range;
        }
    }
}