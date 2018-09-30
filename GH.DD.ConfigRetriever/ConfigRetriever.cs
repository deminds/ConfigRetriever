using System;
using System.Threading.Tasks;

namespace GH.DD.ConfigRetriever
{
    // TODO: debug mode
    public class ConfigRetriever<TItem>
        where TItem : class, new()
    {
        private IRetriever _retriever;
        private IConfigWalker _walker;
        private ConvertProvider _convertProvider;
        private IConfigMapper _configMapper;
        
        public ConfigRetriever(IRetriever retriever)
        {
            _retriever = retriever ?? throw new ArgumentNullException(nameof(retriever));
            _walker = new ConfigWalker<TItem>();
            _convertProvider = new ConvertProvider();
            _configMapper = new ConfigMapper<TItem>();
        }

        public async Task<TItem> Fill()
        {
            foreach (var configElement in _walker.Walk())
            {
                var rawValue = "";
                var configElementWasFound = false; 
                foreach (var path in configElement.GetNextPath())
                {
                    rawValue = await _retriever.Retrieve(path);
                    if (string.IsNullOrWhiteSpace(rawValue)) 
                        continue;
                    
                    configElementWasFound = true;
                    break;
                }
                
                if (!configElementWasFound)
                    throw new EntryPointNotFoundException($"Config element not found. ConfigElement: {configElement}");
                
                if (string.IsNullOrEmpty(rawValue))
                    continue;

                var value = _convertProvider
                                .GetConverter(configElement.ElementType)
                                .Convert(rawValue);

                _configMapper.Map(configElement.PathInConfigObject, value);

            }

            return (TItem)_configMapper.GetResultObject();
        }
    }
}