using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WooliesXTest.Data.Models;

namespace WooliesXTest.Services.Services
{
    public interface IProductsService
    {
        Task<List<Product>> GetAsync();
    }

    public class ProductsService : IProductsService
    {
        private readonly HttpClient _client;
        private readonly IShopperHistoryService _shopperHistoryService;

        public ProductsService(HttpClient client, IShopperHistoryService shopperHistoryService)
        {
            _client = client;
            _shopperHistoryService = shopperHistoryService;
        }

        public async Task<List<Product>> GetAsync()
        {
            var httpResponse = await _client.GetAsync($"resource/products");
            var products = await httpResponse.Content.ReadAsAsync<List<Product>>();
            return products;
        }
    }
}
