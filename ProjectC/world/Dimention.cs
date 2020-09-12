using System;
using System.Collections;
using System.Collections.Generic;
using System.Json;
using Microsoft.Xna.Framework;
using ProjectC.objects;

namespace ProjectC.world
{
    public class Dimention : IStorable
    {
        public WorldType DimentionWorldType = WorldType.Overworld;
        public static Dimention Current = new Dimention();
        private List<IStorable> _chunks = new List<IStorable>();
        private List<IStorable> _gameobjects = new List<IStorable>();
        private List<IStorable> _tiles = new List<IStorable>();
        private Dictionary<Point,Chunk> ChunkDict = new Dictionary<Point, Chunk>();
        public static Chunk VoidChunk = new Chunk(Point.Zero, true);
        public static Tile VoidTile = new Tile(EnumTiles.Fresh, VoidChunk, Point.Zero);
        
        public List<IStorable> AllLoadedThings(EnumStorables thingtype)
        {
            switch (thingtype)
            {
                case EnumStorables.Chunks:
                    return _chunks;
                
                case EnumStorables.Tiles:
                    return _tiles;
                
                default:
                    return _gameobjects;
            }
        }

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
            return LoadChunk(new Vector2((position.X / Tile.TileSize) / Chunk.ChunkWidth, (position.Y / Tile.TileSize) / Chunk.ChunkHeight).ToPoint(), generate);
        }

        public Tile TileAtWorldPos(Vector2 position, bool generate)
        {
            if (position.X < 0 || position.Y < 0)
            {
                return VoidTile;
            }
            var chunk = ChunkAtWorldPos(position, generate);
            var cpos = chunk.WorldToChunk(position);
            return chunk.TileFrom(cpos);
        }

        public Chunk LoadChunk(Point chunkpos, bool generate)
        {
            if (chunkpos.X < 0 || chunkpos.Y < 0)
            {
                return VoidChunk;
            }
            if (ChunkDict.TryGetValue(chunkpos, out var chunk))
            {
                if (!_chunks.Contains(chunk))
                {
                    LoadChunk(chunk);
                }
                return chunk;
            }

            var chonk = new Chunk(chunkpos);
            if (generate)
            {
                WorldGenerator.GenerateChunk(DimentionWorldType, chonk);
            }
            if (!_chunks.Contains(chonk))
            {
                LoadChunk(chonk);
            }
            ChunkDict.Add(chunkpos,chonk);
            return chonk;
        }

        public static void LoadChunk(Chunk chunk)
        {
            Current._chunks.Add(chunk);
        }
        
        public static void UnloadGameObject(GameObject obj)
        {
            Current._gameobjects.Remove(obj);
        }

        public static void LoadGameObject(GameObject obj)
        {
            Current._gameobjects.Add(obj);
        }
        
        public static void UnloadTile(Tile  obj)
        {
            Current._tiles.Remove(obj);
        }

        public static void LoadTile(Tile obj)
        {
            Current._tiles.Add(obj);
        }
    }
}