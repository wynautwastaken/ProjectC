using System;
using Microsoft.Xna.Framework;

namespace ProjectC.world
{
    public class WorldGenerator
    {
        public static void GenerateChunk(WorldType worldType, Chunk chunk)
        {
            for (var i = 0; i < Chunk.ChunkWidth; i++)
            {
                var n = NoiseGenerator.GenNoise(chunk.Position.X, (Chunk.ChunkHeight));
                n = Math.Clamp(n, 0, Chunk.ChunkHeight-1);
                for (var j = Chunk.ChunkHeight-1; j < n; j--)
                {
                    var pos = new Point(i, j);
                    Console.WriteLine(pos.ToString());
                    new Tile(EnumTiles.Fresh, chunk, pos);
                }
            }
        }
    }
}