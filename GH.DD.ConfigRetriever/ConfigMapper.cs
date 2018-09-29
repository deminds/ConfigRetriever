using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GH.DD.ConfigRetriever
{
    public class ConfigMapper<TItem> : IConfigMapper 
        where TItem : class, new()
    {
        private TItem _config;
        
        // Dictionary propertyName on ConfigMapper
        private Dictionary<string, IConfigMapper> _children;

        public ConfigMapper()
        {
            _config = new TItem();
            _children = new Dictionary<string, IConfigMapper>();
        }

        public object GetResultObject()
        {
            foreach (var child in _children)
            {
                var propertyName = child.Key;
                var childConfigMapper = child.Value;

                var propertyInfo = typeof(TItem).GetProperty(propertyName)
                    ?? throw new NullReferenceException($"Property: {propertyName} not found in {typeof(TItem).Name}");
                
                propertyInfo.SetValue(_config, childConfigMapper.GetResultObject());
            }

            return _config;
        }

        public void Map(List<string> path, object value)
        {
            // path[0] - TItem name
            // path[1] - nested class name OR property name

            if (path.Count < 2)
                throw new MemberAccessException($"Error. Try map value ({value}) by path [{string.Join("/", path)}]. " +
                                                $"Mapper must have path with count gt 2");

            var propertyName = path[1];
            var propertyInfo = typeof(TItem).GetProperty(propertyName)
                    ?? throw new NullReferenceException($"Property: {propertyName} not found in {typeof(TItem).Name}");
            
            if (path.Count == 2)
            {
                try
                {
                    propertyInfo.SetValue(_config, value);
                }
                catch (Exception)
                {
                    throw new DataException($"Can not set value: {value} of type: {value.GetType()}" +
                                            $" to property: {propertyName} of type: {propertyInfo.PropertyType}");
                }
                
                return;
            }

            var propertyType = propertyInfo.PropertyType;
            if (propertyType.IsPrimitive || (propertyType == typeof(string)))
            {
                throw new MemberAccessException($"Error. Try map value of nested element of non class property. " +
                                                $"PropertyType: {propertyType.Name}. Path: {path}. Value: {value}");
            }
            
            var nestedPath = path.ToList();
            nestedPath.RemoveAt(0);

            if (!_children.ContainsKey(propertyName))
            {
                var typeArgs = new[] {propertyType};
                var configMapperType = typeof(ConfigMapper<>).MakeGenericType(typeArgs);

                var configMapper = (IConfigMapper) Activator.CreateInstance(configMapperType);

                _children.Add(propertyName, configMapper);
            }

            _children[propertyName].Map(nestedPath, value);
        }
    }
}