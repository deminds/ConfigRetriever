using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace GH.DD.ConfigRetriever.Retrievers
{
    /// <summary>
    /// Class for retrieve data from Consul parse it and return string value
    /// </summary>
    public class ConsulRetriever : IRetriever
    {
        private string _baseUrl;
        private HttpClient _httpClient;
        
        /// <summary>
        /// Constructor for <see cref="ConsulRetriever"/>
        /// </summary>
        /// <param name="httpSchema">Consul http schema (HTTP or HTTPS)</param>
        /// <param name="host">Consul host</param>
        /// <param name="port">Consul port</param>
        /// <param name="aclToken">Consul ACL token. Not required</param>
        /// <exception cref="Exception">Need catch exceptions</exception>
        public ConsulRetriever(string httpSchema, string host, int port, string aclToken)
        {
            if (httpSchema != "http" && httpSchema != "https")
                throw new ArgumentException(nameof(httpSchema));
            
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException(nameof(host));

            if (port < IPEndPoint.MinPort || port >= IPEndPoint.MaxPort)
                throw new ArgumentException(nameof(port));

            _baseUrl = $"{httpSchema}://{host}:{port}/v1/kv/";
            _httpClient = InitializeHttpClient(aclToken);
        }

        /// <summary>
        /// Constructor for <see cref="ConsulRetriever"/>
        /// </summary>
        /// <param name="url">URL for Consul. Example: "https://domain.com:8085"</param>
        /// <param name="aclToken">Consul ACL token. Not required</param>
        /// <exception cref="Exception">Need catch exceptions</exception>
        public ConsulRetriever(string url, string aclToken)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException(nameof(url));
            
            _baseUrl = $"{url}/v1/kv/";
            _httpClient = InitializeHttpClient(aclToken);
        }

        /// <summary>
        /// Retrieve data from Consul via http
        /// </summary>
        /// <param name="path">Path to Consul data in key/value store. <see cref="IList{string}"/></param>
        /// <returns>Consul data value</returns>
        /// <exception cref="Exception">Need catch exceptions</exception>
        public async Task<string> Retrieve(IList<string> path)
        {
            var url = $"{_baseUrl}/{string.Join("/", path)}";

            using (var response = await _httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseJsonRaw = await response.Content.ReadAsStringAsync();
                var responseJson = ParseResponseJson(responseJsonRaw);
                if (responseJson.Count != 1)
                    throw new DataException($"Received wrong response from consul. Count != 1. " +
                                            $"Path: {path}. URL: {url}. RawJson: {responseJsonRaw}");
                
                var result = Base64Decode(responseJson[0].Value);

                return result;
            }
        }

        private HttpClient InitializeHttpClient(string aclToken)
        {
            var httpClient = new HttpClient();
            
            if (!string.IsNullOrWhiteSpace(aclToken))
                httpClient.DefaultRequestHeaders.Add("X-Consul-Token", aclToken);

            return httpClient;
        }

        private List<ConsulResponseObject> ParseResponseJson(string jsonRaw)
        {
            List<ConsulResponseObject> result;
            using (var jsonRawStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonRaw)))
            {
                try
                {
                    var serializer = new DataContractJsonSerializer(typeof(List<ConsulResponseObject>));
                    result = serializer.ReadObject(jsonRawStream) as List<ConsulResponseObject>;
                }
                catch (Exception e)
                {
                    throw new DataException($"Parse consul response json error. Raw JSON: {jsonRaw}. Error: {e}", e);
                }
            }
            
            if (result == null)
                throw new DataException($"Error cast rawJson to collection of ConsulResponseObject. RawJson: {jsonRaw}");
            
            return result;
        }

        private string Base64Decode(string rawData)
        {
            string result;
            try
            {
                var bytesData = Convert.FromBase64String(rawData);
                result = Encoding.UTF8.GetString(bytesData);
            }
            catch (Exception e)
            {
                throw new DataException($"Error convert consul raw data from base64 to utf8 string", e);
            }

            return result;
        }
    }
}
