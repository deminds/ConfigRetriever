using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    public interface IConfigMapper
    {
        void Map(List<string> path, object value);
        object GetResultObject();
    }
}