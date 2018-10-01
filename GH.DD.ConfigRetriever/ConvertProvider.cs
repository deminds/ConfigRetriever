using System;
using System.Collections.Generic;
using GH.DD.ConfigRetriever.Converters;

namespace GH.DD.ConfigRetriever
{
    // ToDo: may be IConverter to delegate
    /// <summary>
    /// Provider for register converters from string to different types and get converter by types
    /// </summary>
    public class ConvertProvider
    {
        private Dictionary<Type, IConverter> _converters;

        /// <summary>
        /// Constructor for <see cref="ConvertProvider"/>
        /// </summary>
        public ConvertProvider()
        {
            _converters = new Dictionary<Type, IConverter>();
            RegisterStandartConverters();
        }

        /// <summary>
        /// Retrieve converter for specific type
        /// Coverter for convert from string to convertToType
        /// </summary>
        /// <param name="convertToType">Type for convert to</param>
        /// <returns><see cref="IConverter"/></returns>
        /// <exception cref="InvalidCastException"></exception>
        public IConverter GetConverter(Type convertToType)
        {
            if (!_converters.TryGetValue(convertToType, out var result))
                throw new InvalidCastException($"Converter for type {convertToType.Name} not found");

            return result;
        }

        /// <summary>
        /// Register custom converter
        /// </summary>
        /// <param name="convertToType"></param>
        /// <param name="converter"></param>
        public void RegisterConverter(Type convertToType, IConverter converter)
        {
            if (_converters.ContainsKey(convertToType))
                _converters.Remove(convertToType);
            
            _converters.Add(convertToType, converter);
        }

        private void RegisterStandartConverters()
        {
            RegisterConverter(typeof(int), new StringToIntConverter());
            RegisterConverter(typeof(long), new StringToLongConverter());
            RegisterConverter(typeof(double), new StringToDoubleConverter());
            RegisterConverter(typeof(bool), new StringToBoolConverter());
            RegisterConverter(typeof(string), new StringToStringConverter());
        }
    }
}