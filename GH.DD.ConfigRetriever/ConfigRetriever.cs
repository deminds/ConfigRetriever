using System;
using System.Threading.Tasks;
using GH.DD.ConfigRetriever.Loggers;

namespace GH.DD.ConfigRetriever
{
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
        
        private readonly ILogger _defaultLogger = new StubLogger();
        private ILogger _logger;
        
        /// <summary>
        /// Main constructor for ConfigRetriever
        /// </summary>
        /// <param name="retriever"><see cref="IRetriever"/>
        /// Interface for retrieve data from configuration system.
        /// Usualy <see cref="GH.DD.ConfigRetriever.Retrievers.ConsulRetriever"/>
        /// for retrieve from Consul</param>
        public ConfigRetriever(IRetriever retriever, ILogger logger = null)
        {
            _retriever = retriever ?? throw new ArgumentNullException(nameof(retriever));
            _walker = new ConfigWalker<TItem>();
            _convertProvider = new ConvertProvider();
            _configMapper = new ConfigMapper<TItem>();

            _logger = logger;
            if (logger == null)
                _logger = _defaultLogger;
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
                _logger.Debug($"> Proceed config element: {configElement}");
                
                var rawValue = "";
                var configElementWasFound = false; 
                foreach (var path in configElement.GetNextPath())
                {
                    _logger.Debug($"Try retrieve from Consul path: \"/{string.Join("/", path)}\"");
                    rawValue = await _retriever.Retrieve(path);
                    if (string.IsNullOrWhiteSpace(rawValue)) 
                        continue;
                    
                    _logger.Debug($"Value was found by path: \"/{string.Join("/", path)}\". Raw value: \"{rawValue}\"");
                    configElementWasFound = true;
                    break;
                }
                
                if (!configElementWasFound)
                    throw new EntryPointNotFoundException($"Config element not found");
                
                if (string.IsNullOrEmpty(rawValue))
                    continue;

                _logger.Debug($"Try convert value to type: {configElement.ElementType}");
                var value = _convertProvider
                                .GetConverter(configElement.ElementType)
                                .Convert(rawValue);
                
                _logger.Debug($"Map to config object by path: \"/{string.Join("/", configElement.PathInConfigObject)}\". Value: \"{value}\"");
                _configMapper.Map(configElement.PathInConfigObject, value);
            }

            return (TItem)_configMapper.GetResultObject();
        }
    }
}