using System.Json;

namespace ProjectC.Universal.World
{
    public interface IChunkStorable
    {
        public JsonObject Save();
        public bool Load(string data);
    }
}