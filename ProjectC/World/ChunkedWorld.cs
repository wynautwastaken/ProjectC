using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Json;
using System.Linq;
using Microsoft.Xna.Framework;
using ProjectC.Engine.Objects;
using SharpDX.Direct3D11;

namespace ProjectC.World
{
    public class ChunkedWorld
    {
        public static readonly Dictionary<ChunkIdentifier, Chunk> Chunks = new Dictionary<ChunkIdentifier, Chunk>();
        public static ChunkedWorld Instance = new ChunkedWorld();
        public static Chunk[] ChunksLoaded;
        public static WorldGenerator WorldGen;
        
        private ChunkedWorld()
        {
            ChunksLoaded = new Chunk[256];
            Load();
        }

        public static void Save()
        {
            var json = new JsonObject();
            var chunkarr = new JsonArray();
            foreach (var chunk in Chunks.Values)
            {
                chunkarr.Add(chunk.Save());
            }
            json.Add("world",chunkarr);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/save.json",json.ToString());
        }

        public static void Load()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/save.json"))
            {
                var filejson = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                "/save.json");
                var json = JsonValue.Parse(filejson);
                if (((JsonObject) json).TryGetValue("world", out var chunkarr))
                {
                    foreach (var obj in (JsonArray) chunkarr)
                    {
                        var chunk = new Chunk((JsonObject) obj);
                        LoadChunk(chunk.ChunkId, chunk);
                    }
                }
            }
            else
            {
                WorldGen = new WorldGenerator();
            }
        }

        public static Chunk LoadChunk(ChunkIdentifier chunkid)
        {
            Chunks.TryGetValue(chunkid, out var chunk);
            if (Chunks.ContainsKey(chunkid)) return chunk;
            var chonk = new Chunk(chunkid.Pos);
            Chunks.Add(chunkid, chonk);
            var ind = NewIndex(ChunksLoaded);
            ChunksLoaded[ind] = chonk;
            return chonk;
        }

        public static bool IsChunkLoaded(Chunk chunk)
        {
            return ChunksLoaded.Any(c => c == chunk);
        }
        public static bool DoesChunkExist(ChunkIdentifier pos)
        {
            return Chunks.ContainsKey(pos);
        }
        
        public static int NewIndex(Array arr)
        {
            return Array.IndexOf(arr, null);
        }

        public static Chunk LoadChunk(ChunkIdentifier chunkid, Chunk chunk)
        {
            if (!Chunks.ContainsKey(chunkid))
            {
                Chunks.Add(chunkid, chunk);
            }

            if (!IsChunkLoaded(chunk))
            {
                ChunksLoaded[NewIndex(ChunksLoaded)] = chunk;
            }
            return chunk;
        }
    }
}