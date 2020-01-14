using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.Data.Enums;
using WooliesXTest.Data.Models;
using WooliesXTest.Services.Interfaces;

namespace WooliesXTest.Services.Services.ProductSort
{
    public class RecommendedSortService : IProductSortService
    {
        private readonly IProductsService _productsService;
        private readonly IShopperHistoryService _shopperHistoryService;

        public RecommendedSortService(IProductsService productsService,
            IShopperHistoryService shopperHistoryService)
        {
            _shopperHistoryService = shopperHistoryService;
            _productsService = productsService;
        }
        public SortOptions SortType => SortOptions.Recommended;

        public async Task<List<Product>> SortAsync()
        {
            var products = await _productsService.GetAsync();

            var shopperHistory = await _shopperHistoryService.GetAsync();

            var productRanks = shopperHistory.SelectMany(x => x.Products).GroupBy(x => x.Name).Select(
                g => new
                {
                    Name = g.First().Name,
                    Quantity = g.Sum(p => p.Quantity)
                }).OrderBy(x => x.Quantity).Select(x => x.Name).ToList();

            var result = products.OrderByDescending(p => productRanks.IndexOf(p.Name)).ToList();

            return result;
        }
    }
}