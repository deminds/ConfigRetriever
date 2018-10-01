using System.IO;

namespace GH.DD.ConfigRetriever.Converters
{
    internal class StringToBoolConverter : IConverter
    {
        public object Convert(string rawValue)
        {
            if (!bool.TryParse(rawValue, out var result))
                throw new InvalidDataException($"Can not convert string to bool. RawValue: {rawValue}");

            return result;
        }
    }
}