using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverPathAttribute : Attribute
    {
        public IList<string> Path { get; }

        public ConfigRetrieverPathAttribute(params string[] pathLevels)
        {
            if (!IsValid(pathLevels))
                throw new ArgumentException("Some path of Path list is null or empty");
            
            Path = pathLevels;
        }

        private bool IsValid(IList<string> path)
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