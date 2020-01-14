using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WooliesXTest.Data.Models;

namespace WooliesXTest.Services.Services
{
    public interface IShopperHistoryService
    {
        Task<List<ShopperHistory>> GetAsync();
    }
    public class ShopperHistoryService : IShopperHistoryService
    {
        private readonly HttpClient _client;

        public ShopperHistoryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ShopperHistory>> GetAsync()
        {
            var httpResponse = await _client.GetAsync($"resource/shopperHistory");
            var history = await httpResponse.Content.ReadAsAsync<List<ShopperHistory>>();
            return history;
        }
    }
}
