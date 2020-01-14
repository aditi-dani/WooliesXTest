using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WooliesXTest.Data.Models;

namespace WooliesXTest.Services.Services
{
    public class ExternalTrolleyService : ITrolleyService
    {
        private readonly HttpClient _client;

        public ExternalTrolleyService(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// This method calls the WooliesX trolleyTotal API
        /// </summary>
        /// <param name="trolleyRequest"></param>
        /// <returns></returns>
        public async Task<decimal> CalculateTrolleyTotal(TrolleyRequest trolley)
        {

            var content = new StringContent(JsonConvert.SerializeObject(trolley), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync($"resource/trolleyCalculator", content);
            var products = await httpResponse.Content.ReadAsAsync<decimal>();
            return products;
        }
    }
}