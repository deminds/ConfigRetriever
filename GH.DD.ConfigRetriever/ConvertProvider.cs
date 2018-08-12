using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    public class ConvertProvider
    {
        private Dictionary<Type, IConverter> _converters;

        public ConvertProvider()
        {
            _converters = new Dictionary<Type, IConverter>();
            RegisterStandartConverters();
        }

        public IConverter GetConverter(Type convertToType)
        {
            if (!_converters.TryGetValue(convertToType, out var result))
                throw new InvalidCastException($"Converter for type {convertToType.Name} not found");

            return result;
        }

        public void RegisterConverter(Type convertToType, IConverter converter)
        {
            if (_converters.ContainsKey(convertToType))
                _converters.Remove(convertToType);
            
            _converters.Add(convertToType, converter);
        }
        
        private void RegisterStandartConverters()
        {}
    }
}