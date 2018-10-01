namespace GH.DD.ConfigRetriever
{
    /// <summary>
    /// Interface for type converter from string to specific type
    /// For use you should register it in <see cref="ConvertProvider"/>
    /// </summary>
    public interface IConverter
    {
        object Convert(string rawValue);
    }
}