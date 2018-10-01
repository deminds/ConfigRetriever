using System;
using System.Threading.Tasks;

namespace GH.DD.ConfigRetriever
{
    // TODO: debug mode
    // TODO: create extensions for easy run and get consul url from env
    /// <summary>
    /// Class for retrieve you config from config system (for example Consul) and map it on you config object 
    /// </summary>
    /// <typeparam name="TItem">Type of you config object</typeparam>
    public class ConfigRetriever<TItem>
        where TItem : class, new()
    {
        private IRetriever _retriever;
        private IConfigWalker _walker;
        private ConvertProvider _convertProvider;
        private IConfigMapper _configMapper;
        
        /// <summary>
        /// Main constructor for ConfigRetriever
        /// </summary>
        /// <param name="retriever"><see cref="IRetriever"/>
        /// Interface for retrieve data from configuration system.
        /// Usualy <see cref="GH.DD.ConfigRetriever.Retrievers.ConsulRetriever"/>
        /// for retrieve from Consul</param>
        public ConfigRetriever(IRetriever retriever)
        {
            _retriever = retriever ?? throw new ArgumentNullException(nameof(retriever));
            _walker = new ConfigWalker<TItem>();
            _convertProvider = new ConvertProvider();
            _configMapper = new ConfigMapper<TItem>();
        }

        /// <summary>
        /// Fill out config object
        /// </summary>
        /// <returns>Filled config object of type <see cref="TItem"/></returns>
        /// <exception cref="Exception">Need catch exceptions</exception>
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