using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectC.Universal.Networking.Packets
{
    public class BufferReader
    {
        private readonly BinaryReader reader;
        public int Position => (int)reader.BaseStream.Position;

        public BufferReader(byte[] array) {
            reader = new BinaryReader(new MemoryStream(array));
        }

        public string ReadString() {
            List<byte> byteList = new List<byte>();
            while (true) {
                byte readByte = reader.ReadByte();
                if (readByte == 0) {
                    break;
                }
                byteList.Add(readByte);
            }

            return Encoding.UTF8.GetString(byteList.ToArray());
        }

        public T ReadEnum<T>() where T : Enum {
            byte b = reader.ReadByte();
            if (Enum.GetUnderlyingType(typeof(T)) == typeof(int)) {
                int i = b;
                return Unsafe.As<int, T>(ref i);
            }
            else {
                return Unsafe.As<byte, T>(ref b);
            }
        }


        public Vector2 ReadVector() {
            Vector2 vec;
            vec.X = this.ReadFloat();
            vec.Y = this.ReadFloat();
            return vec;
        }

        public long ReadInt64() => reader.ReadInt64();

        public int ReadInt32() => reader.ReadInt32();

        public short ReadInt16() => reader.ReadInt16();

        public byte ReadByte() => reader.ReadByte();

        public bool ReadBool() => reader.ReadBoolean();

        public float ReadFloat() => reader.ReadSingle();

        public double ReadDouble() => reader.ReadDouble();

        public byte[] ReadBytes(int count) => reader.ReadBytes(count);

        public void ResetPosition() {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
        }

    }
}