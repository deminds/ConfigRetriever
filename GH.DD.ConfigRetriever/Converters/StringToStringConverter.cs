namespace GH.DD.ConfigRetriever.Converters
{
    internal class StringToStringConverter : IConverter
    {
        public object Convert(string rawValue)
        {
            return rawValue;
        }
    }
}