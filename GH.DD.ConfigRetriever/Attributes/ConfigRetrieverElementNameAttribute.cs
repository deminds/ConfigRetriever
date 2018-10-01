using System;

namespace GH.DD.ConfigRetriever.Attributes
{
    /// <summary>
    /// Attribute for set custom name for you config element in Consul
    /// May be use on root config class and on property
    /// </summary>
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