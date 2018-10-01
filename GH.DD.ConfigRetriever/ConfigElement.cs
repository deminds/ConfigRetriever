using System;
using System.Collections.Generic;
using System.Text;

namespace GH.DD.ConfigRetriever
{
    /// <summary>
    /// Element of config
    /// Is parsed from you config class by <see cref="ConfigWalker{TItem}"/>
    /// </summary>
    public class ConfigElement
    {
        /// <summary>
        /// Paths to config element in Consul
        /// last element is element name
        /// higher index - higher priority
        /// </summary>
        internal List<List<string>> Paths { get; }
        
        /// <summary>
        /// Path to config element in config class
        /// </summary>
        internal List<string> PathInConfigObject { get; }
        
        /// <summary>
        /// Type of config element
        /// </summary>
        internal Type ElementType { get; }

        /// <summary>
        /// Constructor for <see cref="ConfigElement"/>
        /// </summary>
        /// <param name="paths">
        /// Paths to config element in Consul
        /// last element is element name
        /// higher index - higher priority
        /// </param>
        /// <param name="pathInConfigObject">Path to config element in config class</param>
        /// <param name="elementType">Type of config element in config object</param>
        /// <exception cref="Exception">Need catch exceptions</exception>
        public ConfigElement(List<List<string>> paths, 
                             List<string> pathInConfigObject,
                             Type elementType)
        {
            // TODO: maybe check type only in configProvider?
            if (elementType != typeof(string) &&
                elementType != typeof(int) &&
                elementType != typeof(long) &&
                elementType != typeof(double) &&
                elementType != typeof(bool))
                throw new ArgumentException($"ConfigElement field ElementType must be: string, int, long or double. ElementType is {elementType}");
            
            Paths = paths ?? throw new ArgumentNullException(nameof(paths));
            if (Paths.Count == 0)
                throw new ArgumentException($"Paths is empty");
            
            PathInConfigObject = pathInConfigObject ?? throw new ArgumentNullException(nameof(pathInConfigObject));
            if (PathInConfigObject.Count == 0)
                throw new ArgumentException($"PathInConfigObject is empty");
            
            ElementType = elementType;
        }

        /// <summary>
        /// Enumenator for retun path for retrieve config element value from Consul in specific priorty
        /// </summary>
        /// <returns>Enumenator for <see cref="List{string}"/></returns>
        public IEnumerable<List<string>> GetNextPath()
        {
            for (var i = Paths.Count - 1; i >= 0; i--)
            {
                yield return Paths[i];
            }
        }

        /// <summary>
        /// Stringify information about config element
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var paths = new StringBuilder();
            var i = 0;
            var count = Paths.Count;
            foreach (var path in Paths)
            {
                paths.Append("\"/");
                paths.Append(string.Join("/", path));
                paths.Append("\"");

                if (++i != count)
                    paths.Append(", ");
            }
            
            return $"ElementType: {ElementType.Name}, " +
                   $"PathInConfigObject: \"/{string.Join("/", PathInConfigObject)}\", " +
                   $"Paths: [{paths}]";
        }
    }
}