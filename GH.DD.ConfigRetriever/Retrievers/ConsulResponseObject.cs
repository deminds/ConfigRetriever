using Newtonsoft.Json;

namespace GH.DD.ConfigRetriever.Retrievers
{
    public class ConsulResponseObject
    {
        [JsonProperty("LockIndex")]
        public int LockIndex;
        
        [JsonProperty("Key")]
        public string Key;
        
        [JsonProperty("Flags")]
        public int Flags;
        
        [JsonProperty("Value")]
        public string Value;
        
        [JsonProperty("CreateIndex")]
        public int CreateIndex;
        
        [JsonProperty("ModifyIndex")]
        public int ModifyIndex;
    }
}