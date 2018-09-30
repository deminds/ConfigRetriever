namespace GH.DD.ConfigRetriever.Converters
{
    public class StringToStringConverter : IConverter
    {
        public object Convert(string rawValue)
        {
            return rawValue;
        }
    }
}