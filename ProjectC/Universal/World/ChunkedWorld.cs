using System;
using System.Collections.Generic;
using System.IO;
using System.Json;

namespace ProjectC.Universal.World
{
    public class ChunkedWorld
    {
        public static readonly Dictionary<ChunkIdentifier, Chunk> Chunks = new Dictionary<ChunkIdentifier, Chunk>();
        public static ChunkedWorld Instance = new ChunkedWorld();
        public static List<Chunk> ChunksLoaded;
        
        private ChunkedWorld()
        {
            ChunksLoaded = new List<Chunk>();
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
        }

        public static Chunk LoadChunk(ChunkIdentifier chunkid)
        {
            Chunks.TryGetValue(chunkid, out var chunk);
            if (!Chunks.ContainsKey(chunkid))
            {
                chunk = new Chunk(chunkid.Pos * 256 * 8);
                Chunks.Add(chunkid, chunk);
                ChunksLoaded.Add(chunk);
            }
            return chunk;
        }
        
        public static Chunk LoadChunk(ChunkIdentifier chunkid, Chunk chunk)
        {
            if (!Chunks.ContainsKey(chunkid))
            {
                Chunks.Add(chunkid, chunk);
            }
            if (!ChunksLoaded.Contains(chunk))
            {
                ChunksLoaded.Add(chunk);
            }

            return chunk;
        }
    }
}