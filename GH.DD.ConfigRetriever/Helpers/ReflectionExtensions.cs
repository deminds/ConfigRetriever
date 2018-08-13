using System;
using System.Reflection;

namespace GH.DD.ConfigRetriever.Helpers
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPropertyInfo<TItem>(this TItem item, string propertyName)
        {
            var propertyInfo = typeof(TItem).GetProperty(propertyName);
            if (propertyInfo==null)
                throw new NullReferenceException($"Property: {propertyName} not found in {typeof(TItem).Name}");

            return propertyInfo;
        }
    }
}