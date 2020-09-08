using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Colorless.Engine.Objects;
using Microsoft.Xna.Framework;

namespace Colorless.World
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
         */
        public Chunk(String data)
        {
            
        }
        
        /**
         * Adds an object to the chunk
         */
        public void AddObject(Vector2 chunkPos, GameObject gameObject)
        {
            Objects.Add(chunkPos.X+"-"+chunkPos.Y,gameObject);
        }
        
        /**
         * Saves the chunk but does not unload it
         */
        public void Save()
        {
            // find chunk file
            String path = "C:\\Users\\dashi\\Desktop\\save\\" + Position.X + "-" + Position.Y + ".json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileStream stream = File.Create(path);
            
        }
    }
}