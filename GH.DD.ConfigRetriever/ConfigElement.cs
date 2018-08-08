using System;
using System.Collections.Generic;

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
            if (elementType != typeof(string) &&
                elementType != typeof(int) &&
                elementType != typeof(long) &&
                elementType != typeof(double) &&
                elementType != typeof(bool))
                throw new ArgumentException($"ConfigElement field ElementType must be: string, int, long or double. ElementType is {elementType}");
            
            Paths = paths ?? throw new ArgumentNullException(nameof(paths));
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
            // todo: need check
            return $"ElementType: {ElementType}, Paths: {string.Join(":", string.Join(",", Paths))}";
        }
    }
}