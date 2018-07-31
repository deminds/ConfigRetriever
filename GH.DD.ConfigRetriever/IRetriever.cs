namespace GH.DD.ConfigRetriever
{
    public interface IRetriever
    {
        bool TryRetrieve(ConfigElement element, out string value);
    }
}