using System.Collections.Generic;
using System.Threading.Tasks;

namespace GH.DD.ConfigRetriever
{
    public interface IRetriever
    {
        Task<string> Retrieve(IList<string> path);
    }
}