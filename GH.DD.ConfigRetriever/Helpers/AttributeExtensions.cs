using System;
using System.Reflection;

namespace GH.DD.ConfigRetriever.Helpers
{
    /// <summary>
    /// Extensions for manage attributes
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// Return attribute value. Attribute must setup on class
        /// </summary>
        /// <param name="type">Type of class</param>
        /// <param name="valueSelector">Callback for return attribute value</param>
        /// <param name="inherit"></param>
        /// <typeparam name="TAttribute">Attribute type</typeparam>
        /// <typeparam name="TValue">Attribute value type</typeparam>
        /// <returns>Value of attribute</returns>
        /// <example>
        /// var result = typeof(SomeClass)
        ///       .GetAttributeValue((ConfigRetrieverElementNameAttribute a) => a.Name, false);
        /// </example>
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
        
        /// <summary>
        /// Return attribute value. Attribute must setup on property
        /// </summary>
        /// <param name="property"><see cref="PropertyInfo"/> of property</param>
        /// <param name="valueSelector">Callback for return attribute value</param>
        /// <param name="inherit"></param>
        /// <typeparam name="TAttribute">Attribute type</typeparam>
        /// <typeparam name="TValue">Attribute value type</typeparam>
        /// <returns>Value of attribute</returns>
        /// <example>
        /// var result = _propertyInfo.GetAttributeValue((ConfigRetrieverFailbackPathAttribute a) => a.FailbackPath, false);
        /// </example>
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

        /// <summary>
        /// Return attribute is assign to class or not
        /// </summary>
        /// <param name="type">Type of class</param>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>That attribute assign on that class = true</returns>
        /// <example>
        /// var result = typeof(SomeClass).HasAttribute&lt;ConfigRetrieverElementNameAttribute&gt;();
        /// </example>
        public static bool HasAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return Attribute.IsDefined(type, typeof(TAttribute));
        }
        
        /// <summary>
        /// Return attribute is assign to property or not
        /// </summary>
        /// <param name="type"><see cref="PropertyInfo"/> of property</param>
        /// <typeparam name="TAttribute">Type of attribute</typeparam>
        /// <returns>That attribute assign on that property = true</returns>
        /// <example>
        /// var result = _propertyInfo.HasAttribute&lt;ConfigRetrieverElementNameAttribute&gt;();
        /// </example>
        public static bool HasAttribute<TAttribute>(this PropertyInfo property)
            where TAttribute : Attribute
        {
            return Attribute.IsDefined(property, typeof(TAttribute));
        }
    }
}