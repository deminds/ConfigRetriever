using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    /// <summary>
    /// Interface for <see cref="ConfigWalker{TItem}"/>
    /// </summary>
    public interface IConfigWalker
    {
        IEnumerable<ConfigElement> Walk();
    }
}