using System;

namespace ProjectC.world
{
    public class NoiseGenerator
    {
        public static int TotalSeeds;
        public static int Seed = 2;

        public NoiseGenerator()
        {
            
        }

        public static int GenNoise(float xx, float range)
        {
            xx /= 16;
            float noise = 0;

            float chunkSize = 32;

            range = range / 2;

            while(chunkSize > 0){
                float chunkIndex = xx / chunkSize;
    
                float prog = (xx % chunkSize) / chunkSize;
    
                float leftRandom = RandomSeed(chunkIndex, range);
                float rightRandom = RandomSeed(chunkIndex + 1, range);
    
                noise += (1-prog)*leftRandom + prog*rightRandom;
    
                chunkSize /= 2;
                chunkSize = (float)Math.Round(chunkSize); 
                range /= 2;
                range = Math.Max(1,(float)Math.Round(range));
            }

            return (int)noise;
        }

        public static float RandomSeed(float index, float range)
        {
            var rand = new Random(Seed * (int) index);
            return (float)rand.NextDouble() * range;
        }
    }
}