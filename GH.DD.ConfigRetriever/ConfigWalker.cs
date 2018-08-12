using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Schema;
using GH.DD.ConfigRetriever.Attributes;
using GH.DD.ConfigRetriever.Helpers;

namespace GH.DD.ConfigRetriever
{
    // todo: mark some elements as internal
    public class ConfigWalker<TItem> : IConfigWalker
        where TItem : class
    {
        // paths with current TItem name
        public List<List<string>> BasePaths { private set; get; }
        public List<string> BasePathInConfigObject { private set; get; }

        public ConfigWalker()
        {
            BasePaths = null;
            BasePathInConfigObject = null;
        }

        public ConfigWalker(List<List<string>> basePaths, List<string> basePathInConfigObject)
        {
            if (!typeof(TItem).GetInterfaces().Contains(typeof(IConfigObject)))
            {
                throw new TypeAccessException(
                    $"Type {typeof(TItem).Name} in config object not inplement interface {typeof(IConfigObject).Name}");
            }

            BasePaths = basePaths;
            BasePathInConfigObject = basePathInConfigObject;
        }

        public IEnumerable<ConfigElement> Walk()
        {
            var basePaths = GetBasePaths();
            var basePathInConfigObject = GetBasePathInConfigObject();

            var properties = typeof(TItem)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (!property.CanRead || !property.CanWrite)
                    continue;

                if (!property.GetMethod.IsPublic || !property.SetMethod.IsPublic)
                    continue;

                var propertyType = property.PropertyType;
                if (property.HasAttribute<ConfigRetrieverIgnoreAttribute>())
                    continue;

                var nameConfigProperty = property.Name;
                var pathInConfigObject = basePathInConfigObject.ToList();
                pathInConfigObject.Add(nameConfigProperty);
                
                var name = property.Name;
                if (property.HasAttribute<ConfigRetrieverElementNameAttribute>())
                {
                    name = property.GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, true);
                }
                
                var paths = new List<List<string>>();
                
                if (property.HasAttribute<ConfigRetrieverPathAttribute>())
                {
                    var path = property.GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, true).ToList();
                    paths.Add(path);
                }
                else
                {
                    paths = basePaths.ToList();
                }

                if (property.HasAttribute<ConfigRetrieverFailbackPathAttribute>())
                {
                    var failbackPath = property.GetAttributeValue((ConfigRetrieverFailbackPathAttribute a) => a.FailbackPath, true).ToList();
                    failbackPath.Add(name);
                    paths.Insert(0, failbackPath);
                }

                if (propertyType.IsClass)
                {
                    var typeArgs = new[] {propertyType};
                    var configWalkerType = typeof(ConfigWalker<>).MakeGenericType(typeArgs);

                    var configWalker = (IConfigWalker) Activator.CreateInstance(configWalkerType, new object[]{paths, pathInConfigObject});

                    foreach (var configElement in configWalker.Walk())
                    {
                        yield return configElement;
                    }
                }
                else
                {
                    yield return new ConfigElement(
                        paths: paths,
                        pathInConfigObject: pathInConfigObject,
                        elementType: propertyType);
                }
            }
        }

        private IList<string> GetBasePathInConfigObject()
        {
            if (BasePathInConfigObject != null && BasePathInConfigObject.Count > 0)
                return BasePathInConfigObject;

            var name = typeof(TItem).Name;
            if (typeof(TItem).HasAttribute<ConfigRetrieverElementNameAttribute>())
            {
                name = typeof(TItem).GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, true);
            }

            BasePathInConfigObject = new List<string>() {name};

            return BasePathInConfigObject;
        }

        private List<List<string>> GetBasePaths()
        {
            if (BasePaths != null && BasePaths.Count > 0)
                return BasePaths;

            var path = new List<string>();
            if (typeof(TItem).HasAttribute<ConfigRetrieverPathAttribute>())
                path = typeof(TItem).GetAttributeValue((ConfigRetrieverPathAttribute a) => a.Path, true).ToList();

            var name = typeof(TItem).Name;
            if (typeof(TItem).HasAttribute<ConfigRetrieverElementNameAttribute>())
            {
                name = typeof(TItem).GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, true);
            }

            path.Add(name);
            
            BasePaths = new List<List<string>>();
            BasePaths.Add(path);

            return BasePaths;
        }
    }
}