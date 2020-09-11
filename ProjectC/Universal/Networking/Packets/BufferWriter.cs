using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectC.Networking.Packets
{
    public class BufferWriter
    {
        private readonly BinaryWriter writer;
        public byte[] Buffer { get; }

        public BufferWriter(int size) {
            Buffer = new byte[size];
            MemoryStream stream = new MemoryStream(Buffer);
            writer = new BinaryWriter(stream);
        }

        public void WriteEnum<T>(T enumValue) where T : Enum {
            byte b = Convert.ToByte(enumValue);
            writer.Write(b);
        }

        public void WriteString(string value) {
            writer.Write(Encoding.UTF8.GetBytes(value));
            byte b = 0;
            writer.Write(b);
        }

        public void WriteVector(Vector2 value) {
            this.WriteFloat(value.X);
            this.WriteFloat(value.Y);
        }

        public void WriteInt64(long value) => writer.Write(value);

        public void WriteInt32(int value) => writer.Write(value);

        public void WriteInt16(short value) => writer.Write(value);

        public void WriteByte(byte value) => writer.Write(value);

        public void WriteFloat(float value) => writer.Write(value);

        public void WriteDouble(double value) => writer.Write(value);

        public void WriteBool(bool value) => writer.Write(value);

        public void WriteBytes(byte[] bytes) => writer.Write(bytes);
        
        public void ResetPosition() {
            writer.Seek(0, SeekOrigin.Begin);
        }
        public int Index => (int)writer.BaseStream.Position;
    }
}