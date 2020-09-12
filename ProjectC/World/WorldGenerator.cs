using Microsoft.Xna.Framework;

namespace ProjectC.world
{
    public class WorldGenerator
    {
        public static void GenerateChunk(WorldType worldType, Chunk chunk)
        {
            for (var i = 0; i < Chunk.ChunkWidth; i++)
            {
                var n = NoiseGenerator.GenNoise(chunk.Position.X, 255f);
                for (var j = Chunk.ChunkHeight-1; j < n; j--)
                {
                    new Tile(EnumTiles.Fresh, chunk, new Point(i, j));
                }
            }
        }
    }
}