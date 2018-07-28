using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverPathAttribute : Attribute
    {
        public IList<string> Path { get; }

        public ConfigRetrieverPathAttribute(IList<string> path)
        {
            if (!IsValid(path))
                throw new ArgumentException("Some path of Path list is null or empty");

            Path = path;
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