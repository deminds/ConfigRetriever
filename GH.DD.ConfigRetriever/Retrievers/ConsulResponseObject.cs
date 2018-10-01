using Newtonsoft.Json;

namespace GH.DD.ConfigRetriever.Retrievers
{
    internal class ConsulResponseObject
    {
        [JsonProperty("LockIndex")]
        internal int LockIndex;
        
        [JsonProperty("Key")]
        internal string Key;
        
        [JsonProperty("Flags")]
        internal int Flags;
        
        [JsonProperty("Value")]
        internal string Value;
        
        [JsonProperty("CreateIndex")]
        internal int CreateIndex;
        
        [JsonProperty("ModifyIndex")]
        internal int ModifyIndex;

        public override string ToString()
        {
            return $"{{ LockIndex: {LockIndex}, Key: {Key}, Flags: {Flags}, " +
                   $"Value: {Value}, CreateIndex: {CreateIndex}, ModifyIndex: {ModifyIndex}}}";
        }
    }
}