using System;
using System.Collections.Generic;
using System.Text;

namespace GH.DD.ConfigRetriever
{
    public class ConfigElement
    {
        // last element is name
        // higher index - higher priority
        public List<List<string>> Paths { private set; get; }
        public List<string> PathInConfigObject { private set; get; }
        public Type ElementType { private set; get; }

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

        public IEnumerable<List<string>> GetNextPath()
        {
            for (var i = Paths.Count - 1; i >= 0; i--)
            {
                yield return Paths[i];
            }
        }

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