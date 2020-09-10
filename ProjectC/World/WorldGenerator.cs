using System;
using Microsoft.Xna.Framework;
using ProjectC.Engine;
using ProjectC.Engine.Objects;
using ProjectC.Objects;

namespace ProjectC.World
{
    public class WorldGenerator
    {
        public static int Seed = 2;
        
        public WorldGenerator()
        {
            float k = 0;
            var rand = new Random(Seed);
            for (var c = 0; c < 10; c++)
            {
                for (var i = 0; i < 256; i++)
                {
                    k += (float)(rand.NextDouble() * 8);
                    var n = GenNoise(k/2, 255f);
                    for (var j = 255; j > n; j--)
                    {
                        var pos = new Vector2(i, j);
                        var chunk = ChunkedWorld.LoadChunk(
                            new ChunkIdentifier(c+(int) (pos.X / 256), 0));
                        var position = chunk.ChunkToWorld(pos);
                        new Tile(EnumTiles.Fresh, chunk, chunk.WorldToChunk(position));
                    }
                }
            }
        }

        public int GenNoise(float xx, float range)
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

        public float RandomSeed(float index, float range)
        {
            var rand = new Random(Seed * (int) index);
            return (float)rand.NextDouble() * range;
        }
    }
}