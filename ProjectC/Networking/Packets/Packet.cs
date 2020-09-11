using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ProjectC.Engine;

namespace ProjectC.Networking.Packets
{
    public class Packet
    {
        /*
         * 2 bytes - data size
         * data
         * 2 bytes - data size
         * data
         * etc.
         */
        
        public PacketType Type;
        public List<object> Data = new List<object>();

        public Packet(PacketType type)
        {
            Type = type;
        }

        public byte[] ByteArray()
        {
            List<byte> bytes = new List<byte>();
            
            // add packet type
            byte[] typeBytes = BitConverter.GetBytes(((int)Type)+1);

            typeBytes = Arithmetic.TrimByteArray(typeBytes);
            
            // add to byte array
            if (typeBytes.Length > 255 && typeBytes.Length < 255*2)
            {
                bytes.Add(255);
                bytes.Add(BitConverter.GetBytes(typeBytes.Length-255)[0]);
            }
            else
            {
                bytes.Add(BitConverter.GetBytes(typeBytes.Length)[0]);
                bytes.Add(0);
            }
            foreach (byte b in typeBytes) {
                bytes.Add(b);
            }

            return bytes.ToArray();
        }
        
        /*
        public static object[] ReadPacket(byte[] bytes)
        {
            
        }
        */

        private static byte[] CreateByteArray(object obj)
        {
            List<byte> bytes = new List<byte>();
            // get object bytes
            byte[] objBytes = null;
            if (obj is int)
            {
                objBytes = BitConverter.GetBytes((int) obj);
            }

            if (objBytes != null)
            {
                objBytes = Arithmetic.TrimByteArray(objBytes);
                
                // create header
                if (objBytes.Length > 255 && objBytes.Length < 255*2)
                {
                    bytes.Add(255);
                    bytes.Add(BitConverter.GetBytes(objBytes.Length-255)[0]);
                }
                else
                {
                    bytes.Add(BitConverter.GetBytes(objBytes.Length)[0]);
                    bytes.Add(0);
                }
                
                // put data in
                foreach (byte b in objBytes) {
                    bytes.Add(b);
                }

                return bytes.ToArray();
            }

            return null;
        }
    }
}