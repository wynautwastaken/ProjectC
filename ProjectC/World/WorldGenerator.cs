using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Minion2D.Helpers;
using ProjectC.objects;

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
            NoiseHelper.Seed = Dimension.Seed;
        }

        public static void GenerateChunk(WorldType worldType, Chunk chunk)
        {
            for (var i = 0; i < Chunk.ChunkWidth; i++)
            {
                var n = NoiseGenerator.GenIntNoise(chunk.Position.X + i, Chunk.ChunkHeight * 10) + Chunk.ChunkHeight * 2;
                for (var j = Chunk.ChunkHeight - 1; j > chunk.WorldToChunk(new Vector2(n)).Y; j--)
                {
                    if(i > Chunk.ChunkWidth || j < 0)
                    {
                        continue;
                    }
                    var pos = new Point(i, j);
                    var worm = chunk.Position + pos.ToVector2();
                    if (!WormHasBeen(worm.X, worm.Y))
                    {
                        TileHelper.TryMakeTile((int)TileTypeFromWorldNoise(n, worm.X, worm.Y), 0, Color.White, 0, chunk, pos);
                    }
                }
            }
        }
        public static EnumTiles TileTypeFromWorldNoise(float noise, float X, float Y)
        {
            var toGrass = Y <= noise + 1;
            var toDirt = Y <= noise + NoiseGenerator.GenFloatNoise(X + Y + noise, 64);
            return toGrass ? EnumTiles.Grass : toDirt ? EnumTiles.Dirt : EnumTiles.Stone;
        }

        public static bool WormHasBeen(float seedI, float seedJ)
        {
            var n = NoiseHelper.GustavsonNoise((seedI / Chunk.ChunkWidth)/8, seedJ / Chunk.ChunkHeight, false, true);
            float n1;
            float add = NoiseGenerator.GenFloatNoise(seedI + seedJ, 4);
            float ratio = Chunk.ChunkWidth / Chunk.ChunkHeight;
            n1 = NoiseHelper.GustavsonNoise(seedI / Chunk.ChunkWidth + add * ratio, seedJ / Chunk.ChunkHeight + add, false, true);
            n = Player.Lerp(n + n1, 0, (Chunk.ChunkHeight * 2) / (seedJ + 1));
            return n >= 1.15f;
        }
    }
}