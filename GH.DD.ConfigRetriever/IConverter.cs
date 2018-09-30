namespace GH.DD.ConfigRetriever
{
    public interface IConverter
    {
        object Convert(string rawValue);
    }
}