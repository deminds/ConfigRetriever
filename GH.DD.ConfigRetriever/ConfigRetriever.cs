using System;
using System.ComponentModel;

namespace GH.DD.ConfigRetriever
{
    public class ConfigRetriever<TItem>
        where TItem : class, new()
    {
        private IRetriever _retriever;
        private TItem _configItem;
        private IConfigWalker _walker;
        
        public ConfigRetriever(IRetriever retriever)
        {
            _retriever = retriever ?? throw new ArgumentNullException(nameof(retriever));
            _configItem = new TItem();
            _walker = new ConfigWalker<TItem>();
        }

        public TItem Fill()
        {
            foreach (var configElement in _walker.Walk())
            {
                var value = "";
                foreach (var path in configElement.GetNextPath())
                {
                    if (_retriever.TryRetrieve(path, out value))
                        break;
                }
                
                if (string.IsNullOrEmpty(value))
                    continue;

                // move to mapper
//                var converter = TypeDescriptor.GetConverter(configElement.ElementType);
//                object valueCasted;
//                try
//                {
//                     valueCasted = converter.ConvertFromInvariantString(value);
//                }
//                catch (Exception e)
//                {
//                    throw new InvalidCastException($"Error cast {configElement} value: {value}", e);
//                }
//                
//                var propertyInfo = typeof(TItem).GetProperty(configElement.NameConfigProperty);
//                
//                if (propertyInfo == null)
//                    throw new NullReferenceException($"Property: {configElement.NameConfigProperty} not found");
//                
//                propertyInfo.SetValue(_configItem, valueCasted);
            }

            return _configItem;
        }
    }
}