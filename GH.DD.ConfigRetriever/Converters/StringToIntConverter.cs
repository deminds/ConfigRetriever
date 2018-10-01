using System.IO;

namespace GH.DD.ConfigRetriever.Converters
{
    internal class StringToIntConverter : IConverter
    {
        public object Convert(string rawValue)
        {
            if (!int.TryParse(rawValue, out var result))
                throw new InvalidDataException($"Can not convert string to int. RawValue: {rawValue}");

            return result;
        }
    }
}