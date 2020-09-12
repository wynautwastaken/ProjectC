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
        private List<Chunk> _chunks = new List<Chunk>();
        private List<GameObject> _gameobjects = new List<GameObject>();
        private List<Tile> _tiles = new List<Tile>();
        private Dictionary<Point,Chunk> ChunkDict = new Dictionary<Point, Chunk>();
        
        public List<IStorable> AllLoadedThings(EnumStorables thingtype)
        {
            switch (thingtype)
            {
                case EnumStorables.Chunks:
                    return new List<IStorable>(_chunks);
                
                case EnumStorables.Tiles:
                    return new List<IStorable>(_tiles);
                
                default:
                    return new List<IStorable>(_gameobjects);
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

        public Chunk LoadChunk(Point chunkpos, bool generate)
        {
            Console.WriteLine($"trying to load chunk {chunkpos.ToString()}");
            if (ChunkDict.TryGetValue(chunkpos, out var chunk))
            {
                Console.WriteLine("found chunk in dictionary");
                return chunk;
            }

            Console.WriteLine("couldn't find chunk");
            var chonk = new Chunk(chunkpos);
            if (generate)
            {
                Console.WriteLine("generating chunk");
                WorldGenerator.GenerateChunk(DimentionWorldType, chonk);
            }
            ChunkDict.Add(chunkpos,chonk);
            Console.WriteLine("added chunk to dictionary");
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