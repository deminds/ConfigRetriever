using System.Collections.Generic;

namespace GH.DD.ConfigRetriever
{
    /// <summary>
    /// Interface for <see cref="ConfigMapper{TItem}"/>
    /// </summary>
    public interface IConfigMapper
    {
        void Map(List<string> path, object value);
        object GetResultObject();
    }
}