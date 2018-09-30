using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverFailbackPathAttribute : Attribute
    {
        public IList<string> FailbackPath { get; }

        public ConfigRetrieverFailbackPathAttribute(params string[] pathLevels)
        {
            if (!IsValid(pathLevels))
                throw new ArgumentException("Some path of FailbackPath list is null or empty");
            
            FailbackPath = pathLevels;
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