using System;
using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    public class ConfigElement
    {
        public string Name { private set; get; }
        public string NameInConfigObject { private set; get; }
        public List<string> Path { private set; get; }
        public Type ElementType { private set; get; }
        public bool CanFloatUp { private set; get; }

        public ConfigElement(string name, string nameInConfigObject, List<string> path, Type elementType, bool canFloatUp)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NameInConfigObject = nameInConfigObject ?? throw new ArgumentNullException(nameof(nameInConfigObject));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            CanFloatUp = canFloatUp;
        }

        public IEnumerable<ConfigElement> FloatUp()
        {
            if (!CanFloatUp)
                yield break;

            for (var i = Path.Count; i < 0; i--)
            {
                yield return new ConfigElement(Name, NameInConfigObject, Path.GetRange(0, i), ElementType, CanFloatUp);
            }
        }
    }
}