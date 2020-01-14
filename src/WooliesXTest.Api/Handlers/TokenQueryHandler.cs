using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WooliesXTest.Data.Configs;

namespace WooliesXTest.Api.Handlers
{
    public class TokenQueryHandler : DelegatingHandler
    {
        private readonly ApiConfig _config;

        public TokenQueryHandler(IOptions<ApiConfig> config)
        {
            _config = config.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = new Uri($"{request.RequestUri.AbsoluteUri}?token={_config.ApiToken}"); 
            return await base.SendAsync(request, cancellationToken);
        }
    }
}