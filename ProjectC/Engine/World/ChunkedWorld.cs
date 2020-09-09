using System.Collections.Generic;
using ProjectC.Engine.World;

namespace ProjectC.Engine.World
{
    public class ChunkedWorld
    {
        public static readonly Dictionary<ChunkIdentifier, Chunk> Chunks = new Dictionary<ChunkIdentifier, Chunk>();
        public static ChunkedWorld Instance = new ChunkedWorld();
        public static readonly List<Chunk> ChunksLoaded = new List<Chunk>();
        
        private ChunkedWorld()
        {
            
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
    }
}