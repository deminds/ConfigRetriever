﻿using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text;

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
                string value = null;
                if (!_retriever.TryRetrieve(configElement, out value))
                {
                    foreach (var elementFloatUp in configElement.FloatUp())
                    {
                        if (_retriever.TryRetrieve(configElement, out value))
                            break;
                    }
                }
                
                if (string.IsNullOrEmpty(value))
                    continue;

                var converter = TypeDescriptor.GetConverter(configElement.ElementType);
                object valueCasted;
                try
                {
                     valueCasted = converter.ConvertFromInvariantString(value);
                }
                catch (Exception e)
                {
                    throw new InvalidCastException($"Error cast {configElement} value: {value}", e);
                }
                
                var propertyInfo = typeof(TItem).GetProperty(configElement.NameConfigProperty);
                
                if (propertyInfo == null)
                    throw new NullReferenceException($"Property: {configElement.NameConfigProperty} not found");
                
                propertyInfo.SetValue(_configItem, valueCasted);
            }

            return _configItem;
        }
        
        private static T CastAs<T>(object obj) where T: class, new()
        {
            return obj as T;
        }
        
    }

    public static class ConvertHelper
    {
        public static T ChangeType<T>(this object obj)
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}