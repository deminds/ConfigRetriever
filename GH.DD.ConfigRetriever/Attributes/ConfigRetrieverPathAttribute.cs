using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever.Attributes
{
    /// <summary>
    /// Attribute for set path for find that config element in Consul instead native path
    /// Native path is path defined from config class structure
    /// May be use on root config class and on property
    /// </summary>
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