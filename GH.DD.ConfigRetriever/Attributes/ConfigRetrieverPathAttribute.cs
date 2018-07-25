using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverPathAttribute : Attribute
    {
        public List<string> Path { get; }

        public ConfigRetrieverPathAttribute(List<string> path)
        {
            if (!IsValid(path))
                throw new ArgumentException("Some parh of Path list is null or empty");

            Path = path;
        }

        private bool IsValid(List<string> path)
        {
            foreach (var pathPart in path)
            {
                if (string.IsNullOrWhiteSpace(pathPart))
                    return false;
            }
            
            return true;
        }
    }
}