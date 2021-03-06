﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GH.DD.ConfigRetriever.Attributes;
using GH.DD.ConfigRetriever.Helpers;

namespace GH.DD.ConfigRetriever
{
    /// <summary>
    /// Class for recursivly walk via config object and generate <see cref="ConfigElement"/> for each value
    /// </summary>
    /// <typeparam name="TItem">Type of config object</typeparam>
    public class ConfigWalker<TItem> : IConfigWalker
        where TItem : class, new()
    {
        /// <summary>
        /// Paths to config element in Consul witout property name
        /// All of BasePaths elements contains name of current TItem or ElementName attr as last element
        /// </summary>
        private List<List<string>> BasePaths { set; get; }
        
        /// <summary>
        /// Paths to config element if config object without property name
        /// BasePathInConfigObject contains name of current TItem as last element
        /// </summary>
        private List<string> BasePathInConfigObject { set; get; }

        /// <summary>
        /// Constructor of <see cref="ConfigWalker{TItem}"/>
        /// With set empty value to BasePaths and BasePathInConfigObject
        /// </summary>
        public ConfigWalker()
        {
            CheckWalkType(typeof(TItem));
            BasePaths = new List<List<string>>();
            BasePathInConfigObject = new List<string>();
        }

        /// <summary>
        /// Constructor of <see cref="ConfigWalker{TItem}"/>
        /// </summary>
        /// <param name="basePaths">
        /// Paths to config element in Consul witout property name
        /// Property name will add there
        /// </param>
        /// <param name="basePathInConfigObject">
        /// Paths to config element if config object without property name
        /// Property name will add there
        /// </param>
        public ConfigWalker(List<List<string>> basePaths, List<string> basePathInConfigObject)
        {
            CheckWalkType(typeof(TItem));
            BasePaths = basePaths ?? throw new ArgumentNullException(nameof(basePaths));
            BasePathInConfigObject = basePathInConfigObject ?? throw new ArgumentNullException(nameof(basePathInConfigObject));
        }

        /// <summary>
        /// Enumenator for analisys config object
        /// </summary>
        /// <returns><see cref="ConfigElement"/></returns>
        public IEnumerable<ConfigElement> Walk()
        {
            UpdateBasePathsProp();
            UpdateBasePathInConfigObjectProp();

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
                var pathInConfigObject = BasePathInConfigObject.ToList();
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
                    foreach (var path in BasePaths)
                    {
                        paths.Add(path.ToList());
                    }
                }

                if (property.HasAttribute<ConfigRetrieverFailbackPathAttribute>())
                {
                    var failbackPath = property.GetAttributeValue((ConfigRetrieverFailbackPathAttribute a) => a.FailbackPath, true).ToList();
                    paths.Insert(0, failbackPath);
                }

                foreach (var path in paths)
                {
                    path.Add(name);
                }

                if (propertyType.IsEnum || propertyType.IsPrimitive || propertyType == typeof(string))
                {
                    yield return new ConfigElement(
                        paths: paths,
                        pathInConfigObject: pathInConfigObject,
                        elementType: propertyType);
                }
                else
                {
                    var typeArgs = new[] {propertyType};
                    
                    CheckWalkType(propertyType);
                    
                    var configWalkerType = typeof(ConfigWalker<>).MakeGenericType(typeArgs);

                    var configWalker = (IConfigWalker) Activator.CreateInstance(configWalkerType, new object[]{paths, pathInConfigObject});

                    foreach (var configElement in configWalker.Walk())
                    {
                        yield return configElement;
                    }
                }
            }
        }

        private void CheckWalkType(Type type)
        {
            if (type.IsEnum ||
                type.IsGenericType ||
                type.IsGenericTypeDefinition ||
                type.IsInterface ||
                type.IsPrimitive ||
                type == typeof(string))
            {
                throw new TypeAccessException(
                    $"Type {type.Name} in config object is wrong type. Must be regular class");
            }
                
            if (!type.GetInterfaces().Contains(typeof(IConfigObject)))
            {
                throw new TypeAccessException(
                    $"Type {type.Name} in config object not inplement interface {typeof(IConfigObject).Name}");
            }
        }
        
        private void UpdateBasePathInConfigObjectProp()
        {
            if (BasePathInConfigObject != null && BasePathInConfigObject.Count > 0)
                return;

            var name = typeof(TItem).Name;
            BasePathInConfigObject = new List<string>
            {
                name
            };
        }

        private void UpdateBasePathsProp()
        {
            if (BasePaths != null && BasePaths.Count > 0)
                return;

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
        }
    }
}