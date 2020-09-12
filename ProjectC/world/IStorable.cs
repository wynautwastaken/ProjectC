using System.Json;

namespace ProjectC.world
{
    public interface IStorable
    {
        public JsonObject Save();
        public bool Load(string data);
    }
}