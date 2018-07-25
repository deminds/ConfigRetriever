using System;

namespace GH.DD.ConfigRetriever.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigRetrieverCanFloatUpAttribute : Attribute
    {
        public bool CanFloatUp { set; get; } = true;
    }
}