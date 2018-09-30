using System;

namespace GH.DD.ConfigRetriever.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverElementNameAttribute : Attribute
    {
        public string Name { get; }

        public ConfigRetrieverElementNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }
    }
}