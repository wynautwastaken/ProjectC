using System.Json;

namespace ProjectC.World
{
    public interface IChunkStorable
    {
        public JsonObject Save();
        public bool Load(string data);
    }
}