using System.IO;

namespace GH.DD.ConfigRetriever.Converters
{
    internal class StringToDoubleConverter : IConverter
    {
        public object Convert(string rawValue)
        {
            if (!double.TryParse(rawValue, out var result))
                throw new InvalidDataException($"Can not convert string to double. RawValue: {rawValue}");

            return result;
        }
    }
}