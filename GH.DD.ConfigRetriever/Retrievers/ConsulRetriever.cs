using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GH.DD.ConfigRetriever.Retrievers
{
    public class ConsulRetriever : IRetriever
    {
        private readonly string _httpSchema;
        private readonly string _host;
        private readonly int _port;

        private HttpClient _httpClient;
        
        public ConsulRetriever(string httpSchema, string host, int port, string aclToken)
        {
            if (httpSchema != "http" && httpSchema != "https")
                throw new ArgumentException(nameof(httpSchema));
            
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException(nameof(host));

            if (port < IPEndPoint.MinPort || port >= IPEndPoint.MaxPort)
                throw new ArgumentException(nameof(port));

            _httpSchema = httpSchema;
            _host = host;
            _port = port;

            _httpClient = new HttpClient();
            
            if (!string.IsNullOrWhiteSpace(aclToken))
                _httpClient.DefaultRequestHeaders.Add("X-Consul-Token", aclToken);
        }

        public async Task<string> Retrieve(IList<string> path)
        {
            var url = $"{_httpSchema}://{_host}:{_port}/v1/kv/{string.Join("/", path)}";

            using (var response = await _httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                    return null;

                var responseJsonRaw = await response.Content.ReadAsStringAsync();
                var responseJson = ParseResponseJson(responseJsonRaw);
                if (responseJson.Count != 1)
                    throw new DataException($"Received response from consul with count of objects != 1. " +
                                            $"Count: {responseJson.Count}. Response raw: {responseJsonRaw}");
                
                var result = Base64Decode(responseJson[0].Value);

                return result;
            }
        }

        private List<ConsulResponseObject> ParseResponseJson(string jsonRaw)
        {
            List<ConsulResponseObject> result;
            try
            {
                result = JsonConvert.DeserializeObject<List<ConsulResponseObject>>(jsonRaw);
            }
            catch (Exception e)
            {
                throw new DataException($"Parse consul response json error. Raw JSON: {jsonRaw}. Error: {e}", e);
            }

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
