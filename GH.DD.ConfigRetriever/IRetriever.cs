using System.Collections.Generic;
using System.Threading.Tasks;

namespace GH.DD.ConfigRetriever
{
    /// <summary>
    /// Interface for retrieve data from specific system (Consul)
    /// Use in <see cref="ConfigRetriever{TItem}"/>
    /// </summary>
    public interface IRetriever
    {
        Task<string> Retrieve(IList<string> path);
    }
}