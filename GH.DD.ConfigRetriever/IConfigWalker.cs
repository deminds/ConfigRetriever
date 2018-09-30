using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    public interface IConfigWalker
    {
        IEnumerable<ConfigElement> Walk();
    }
}