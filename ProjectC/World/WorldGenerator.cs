using System;
using Microsoft.Xna.Framework;

namespace ProjectC.world
{
    public class WorldGenerator
    {
        public static WorldGenerator instance = new WorldGenerator();

        private WorldGenerator()
        {

        }

        public void GenerateWorld()
        {
            return;
        }
        
        public static void GenerateChunk(WorldType worldType, Chunk chunk)
        {
            for (var i = 0; i < Chunk.ChunkWidth; i++)
            {
                var n = NoiseGenerator.GenNoise(chunk.Position.X + i, Chunk.ChunkHeight) + Chunk.ChunkHeight * 2;
                for (var j = Chunk.ChunkHeight-1; j > chunk.WorldToChunk(new Vector2(n)).Y; j--)
                {
                    var pos = new Point(i, j);
                    if(Tile.TryMakeTile(EnumTiles.Fresh, chunk, pos, out var tile)) {
                        Tile.UpdateSpriteLater(tile, EnumTiles.Stone);
                    }
                }
            }
        }
    }
}