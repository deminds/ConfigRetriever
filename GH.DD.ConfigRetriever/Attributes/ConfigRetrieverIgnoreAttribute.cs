using System;

namespace GH.DD.ConfigRetriever.Attributes
{
    /// <summary>
    /// Attribute for ignore you config element. Will not analisys and fill from Consul
    /// May be use on property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverIgnoreAttribute : Attribute
    {
    }
}