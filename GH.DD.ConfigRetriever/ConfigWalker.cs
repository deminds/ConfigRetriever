using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GH.DD.ConfigRetriever.Attributes;
using GH.DD.ConfigRetriever.Helpers;

namespace GH.DD.ConfigRetriever
{
    // todo: mark some elements as internal
    public class ConfigWalker<TItem> 
        where TItem : class 
    {
        private IList<string> GetBasePath()
        {
            var path = new List<string>();
            if (typeof(TItem).HasAttribute<ConfigRetrieverPathAttribute>())
                path = typeof(TItem).GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, true).ToList();

            var name = typeof(TItem).Name;
            if (typeof(TItem).HasAttribute<ConfigRetrieverElementNameAttribute>())
            {
                name = typeof(TItem).GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, true);
            }
            
            path.Add(name);

            return path;
        }

        public IEnumerable<ConfigElement> Walk()
        {
            var basePath = GetBasePath();
            
            var properties = typeof(TItem)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                if (property.HasAttribute<ConfigRetrieverIgnoreAttribute>())
                    continue;

                var path = basePath;
                if (property.HasAttribute<ConfigRetrieverPathAttribute>())
                {
                    path = property.GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, true);
                }

                var canFloatUp = property.HasAttribute<ConfigRetrieverCanFloatUpAttribute>();

                var propertyType = property.PropertyType;
                var nameConfigProperty = propertyType.Name;
                var name = nameConfigProperty;
                if (propertyType.IsValueType)
                    name = property.Name;

                yield return new ConfigElement(
                    name: name,
                    nameConfigProperty: nameConfigProperty,
                    path: path.ToList(),
                    elementType: propertyType,
                    canFloatUp: canFloatUp);
            }
        }
    }
}