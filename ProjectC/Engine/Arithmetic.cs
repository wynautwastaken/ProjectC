using System.Collections.Generic;

namespace ProjectC.Engine
{
    public static class Arithmetic
    {
        /**
         * Only trims the end of bytes
         */
        public static byte[] TrimByteArray(byte[] array)
        {
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < array.Length; i++)
            {
                bytes.Add(array[i]);
            }
            
            // trim end
            for (int i = bytes.Count-1; i >= 0; i--)
            {
                if (bytes[i] == 0)
                {
                    bytes.RemoveAt(i);
                }
            }

            return bytes.ToArray();
        }
    }
}