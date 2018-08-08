using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    public interface IRetriever
    {
        bool TryRetrieve(IList<string> path, out string value);
    }
}