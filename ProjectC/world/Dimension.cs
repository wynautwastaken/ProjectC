using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Json;
using Microsoft.Xna.Framework;
using ProjectC.objects;

namespace ProjectC.world
{
    public class Dimension : IStorable
    {
        public WorldType DimensionWorldType = WorldType.Overworld;
        public static Dimension Current = new Dimension();
        public static Stack<GameObject> ToDestroy = new Stack<GameObject>();
        public static Stack<GameObject> ToLoad = new Stack<GameObject>();
        private readonly List<Chunk> _chunks = new List<Chunk>();
        private readonly List<GameObject> _gameobjects = new List<GameObject>();
        private Dictionary<Point,Chunk> ChunkDict = new Dictionary<Point, Chunk>();
        public static Chunk VoidChunk = new Chunk(Point.Zero, true);
        public static int Seed = 694201337;

        public IReadOnlyList<Chunk> Chunks => _chunks;
        public IReadOnlyList<GameObject> GameObjects => _gameobjects;

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

        public int TileAtWorldPos(Vector2 position, int type, bool generate)
        {
            if (position.X < 0 || position.Y < 0)
            {
                return 0;
            }
            if (!generate && !Current.IsChunkLoaded(new Vector2(position.X / Chunk.ChunkWidth, position.Y / Chunk.ChunkHeight).ToPoint()))
            {
                return 0;
            }
            var chunk = ChunkAtWorldPos(position, generate);
            var cpos = chunk.WorldToChunk(position);
            return chunk.TileFrom(cpos, type);
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
            if (generate)
            {
                var chonk = new Chunk(chunkpos);
            
                WorldGenerator.GenerateChunk(DimensionWorldType, chonk); 
                if (ChunkDict.TryAdd(chunkpos, chonk)) return chonk;
                UnloadChunk(chonk);
                return VoidChunk;
            }
            return VoidChunk;
        }

        public static void LoadChunk(Chunk chunk)
        {
            Current._chunks.Add(chunk);
        }
        
        public static void UnloadChunk(Chunk  chunk)
        {
            Current._chunks.Remove(chunk);
        }

        public static void UnloadGameObject(GameObject obj)
        {
            ToDestroy.Push(obj);
        }

        public static void LoadGameObject(GameObject obj)
        {
            ToLoad.Push(obj);
        }

        public static void DestroyThings()
        {
            while (0 < ToDestroy.Count)
            {
                Current._gameobjects.Remove(ToDestroy.Pop());
            }
        }

        public static void LoadThings()
        {
            while (0 < ToLoad.Count)
            {
                Current._gameobjects.Add(ToLoad.Pop());
            }
        }

    }
}