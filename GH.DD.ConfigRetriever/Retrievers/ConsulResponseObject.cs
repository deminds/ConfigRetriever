using System.Runtime.Serialization;

namespace GH.DD.ConfigRetriever.Retrievers
{
    [DataContract]
    internal class ConsulResponseObject
    {
        [DataMember(Name = "LockIndex")]
        internal int LockIndex;
        
        [DataMember(Name = "Key")]
        internal string Key;
        
        [DataMember(Name = "Flags")]
        internal int Flags;
        
        [DataMember(Name = "Value")]
        internal string Value;
        
        [DataMember(Name = "CreateIndex")]
        internal int CreateIndex;
        
        [DataMember(Name = "ModifyIndex")]
        internal int ModifyIndex;

        public override string ToString()
        {
            return $"{{ LockIndex: {LockIndex}, Key: {Key}, Flags: {Flags}, " +
                   $"Value: {Value}, CreateIndex: {CreateIndex}, ModifyIndex: {ModifyIndex}}}";
        }
    }
}