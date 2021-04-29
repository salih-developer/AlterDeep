using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AlterDeep.Logging
{
    public  class ElasticThreadLogger : IThreadLog
    {
        private readonly ILogger<ElasticThreadLogger> _logger;
        private readonly ElasticLowLevelClient _client;

        public ElasticThreadLogger(IConfiguration configuration,ILogger<ElasticThreadLogger> logger)
        {
            _logger = logger;
            Uri node = new Uri(configuration.GetSection("ElasticseachUrl").Value);
            var config = new ConnectionConfiguration(node);
            _client = new ElasticLowLevelClient(config);
        }
       
        public async Task<bool> Insert(object obj)
        {
            try
            {
                    var indexResponse =await _client.IndexAsync<StringResponse>("threadlog", "StringResponse", PostData.Serializable(obj));
                    return indexResponse.Success;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return false;
        }
    }
}
