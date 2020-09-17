using System;
using System.Collections;
using System.Collections.Generic;
using System.Json;
using Microsoft.Xna.Framework;
using ProjectC.objects;

namespace ProjectC.world
{
    public class Dimension : IStorable
    {
        public WorldType DimensionWorldType = WorldType.Overworld;
        public static Dimension Current = new Dimension();
        private readonly List<Chunk> _chunks = new List<Chunk>();
        private readonly List<GameObject> _gameobjects = new List<GameObject>();
        private readonly List<Tile> _tiles = new List<Tile>();
        private Dictionary<Point,Chunk> ChunkDict = new Dictionary<Point, Chunk>();
        public static Chunk VoidChunk = new Chunk(Point.Zero, true);
        public static Tile VoidTile = new Tile(EnumTiles.Stone, VoidChunk, Point.Zero);
        public static int Seed = 2;

        public IReadOnlyList<Chunk> Chunks => _chunks;
        public IReadOnlyList<GameObject> GameObjects => _gameobjects;
        public IReadOnlyList<Tile> Tiles => _tiles;

        public JsonObject Save()
        {
            throw new System.NotImplementedException();
        }

        public bool Load(string data)
        {
            throw new System.NotImplementedException();
        }

        public Chunk ChunkAtWorldPos(Vector2 position, bool generate)
        {
            if (position.X < 0 || position.Y < 0)
            {
                return VoidChunk;
            }
            return LoadChunk(new Vector2(position.X / Chunk.ChunkWidth, position.Y /  Chunk.ChunkHeight).ToPoint(), generate);
        }

        public Tile TileAtWorldPos(Vector2 position, bool generate)
        {
            if (position.X < 0 || position.Y < 0)
            {
                return VoidTile;
            }
            if (!generate && !Current.IsChunkLoaded(new Vector2(position.X / Chunk.ChunkWidth, position.Y / Chunk.ChunkHeight).ToPoint()))
            {
                return VoidTile;
            }
            var chunk = ChunkAtWorldPos(position, generate);
            var cpos = chunk.WorldToChunk(position);
            return chunk.TileFrom(cpos);
        }

        public bool IsChunkLoaded(Point chunkpos)
        {
            return ChunkDict.ContainsKey(chunkpos);
        }

        public Chunk LoadChunk(Point chunkpos, bool generate)
        {
            if (chunkpos.X < 0 || chunkpos.Y < 0)
            {
                return VoidChunk;
            }
            if (ChunkDict.TryGetValue(chunkpos, out var chunk)) return chunk;
            var chonk = new Chunk(chunkpos);
            if (generate)
            {
                WorldGenerator.GenerateChunk(DimensionWorldType, chonk);
            }
            if (ChunkDict.TryAdd(chunkpos, chonk)) return chonk;
            UnloadChunk(chonk);
            return VoidChunk;
        }

        public static void LoadChunk(Chunk chunk)
        {
            Current._chunks.Add(chunk);
        }
        
        public static void UnloadChunk(Chunk  chunk)
        {
            Current._chunks.Remove(chunk);
            foreach (var tile in chunk.Tiles)
            {
                UnloadTile(tile);
            }
        }
        
        public static void UnloadGameObject(GameObject obj)
        {
            Current._gameobjects.Remove(obj);
        }

        public static void LoadGameObject(GameObject obj)
        {
            Current._gameobjects.Add(obj);
        }
        
        public static void LoadTile(Tile tile)
        {
            Current._tiles.Add(tile);
        }
        
        public static void UnloadTile(Tile tile)
        {
            Current._tiles.Remove(tile);
        }

    }
}