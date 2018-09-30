using System.IO;

namespace GH.DD.ConfigRetriever.Converters
{
    public class StringToLongConverter : IConverter
    {
        public object Convert(string rawValue)
        {
            if (!long.TryParse(rawValue, out var result))
                throw new InvalidDataException($"Can not convert string to long. RawValue: {rawValue}");

            return result;
        }
    }
}