using System;
using System.Reflection;

namespace GH.DD.ConfigRetriever.Helpers
{
    public static class AttributeExtensions
    {

        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type,
            Func<TAttribute, TValue> valueSelector,
            bool inherit)
            where TAttribute : Attribute
        {
            var value = type
                    .GetCustomAttribute(typeof(TAttribute), inherit)
                as TAttribute;

            return value == null ? default(TValue) : valueSelector(value);
        }
        
        public static TValue GetAttributeValue<TAttribute, TValue>(this PropertyInfo property,
            Func<TAttribute, TValue> valueSelector,
            bool inherit)
            where TAttribute : Attribute
        {
            var value = property
                    .GetCustomAttribute(typeof(TAttribute), inherit)
                as TAttribute;

            return value == null ? default(TValue) : valueSelector(value);
        }

        public static bool HasAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return Attribute.IsDefined(type, typeof(TAttribute));
        }
        
        public static bool HasAttribute<TAttribute>(this PropertyInfo property)
            where TAttribute : Attribute
        {
            return Attribute.IsDefined(property, typeof(TAttribute));
        }
    }
}