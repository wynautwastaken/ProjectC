using System;
using System.Collections.Generic;
using System.IO;
using ProjectC.Engine.Objects;
using Microsoft.Xna.Framework;

namespace ProjectC.World
{
    public class Chunk
    {
        public Vector2 Position;
        private Dictionary<String, GameObject> Objects = new Dictionary<string, GameObject>();
        
        /**
         * For creating a new chunk
         */
        public Chunk(Vector2 position)
        {
            Position = position;
        }

        /**
         * Loads a chunk from existing data
         * Not implemented
         */
        public Chunk(String data)
        {
            
        }
        
        /**
         * Adds an object to the chunk
         */
        public void SetPos(Vector2 chunkPos, GameObject gameObject, bool replace)
        {
            string str = chunkPos.X + "-" + chunkPos.Y;
            if (Objects.ContainsKey(str) && replace)
            {
                Objects.Remove(str);
            }
            Objects.Add(chunkPos.X + "-" + chunkPos.Y, gameObject);
        }

        public GameObject FindObject(Vector2 position)
        {
            string str = position.X + "-" + position.Y, gameObject;
            if (Objects.ContainsKey(str))
            {
                return Objects[str];
            }

            return null;
        }
        
        /**
         * Saves the chunk but does not unload it
         */
        public void Save() // TODO unfinished
        {
            // find chunk file
            String path = "C:\\Users\\dashi\\Desktop\\save\\" + Position.X + "-" + Position.Y + ".json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            FileStream stream = File.Create(path);
            // write to stream
        }
    }
}