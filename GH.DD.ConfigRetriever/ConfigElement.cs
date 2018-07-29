using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    public class ConfigElement
    {
        public string Name { private set; get; }
        public string NameConfigProperty { private set; get; }
        public List<string> Path { private set; get; }
        public Type ElementType { private set; get; }
        public bool CanFloatUp { private set; get; }

        public ConfigElement(string name, string nameConfigProperty, List<string> path, Type elementType, bool canFloatUp)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NameConfigProperty = nameConfigProperty ?? throw new ArgumentNullException(nameof(nameConfigProperty));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            CanFloatUp = canFloatUp;
        }

        public IEnumerable<ConfigElement> FloatUp()
        {
            if (!CanFloatUp)
                yield break;

            for (var i = Path.Count - 1; i >= 0; i--)
            {
                yield return new ConfigElement(
                    Name, 
                    NameConfigProperty, 
                    Path.GetRange(0, i), 
                    ElementType, 
                    CanFloatUp);
            }
        }
    }
}