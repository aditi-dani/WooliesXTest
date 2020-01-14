using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.Data.Enums;
using WooliesXTest.Data.Models;
using WooliesXTest.Services.Interfaces;

namespace WooliesXTest.Services.Services.ProductSort
{
    public class DescendingSortService : IProductSortService
    {
        private readonly IProductsService _productsService;
        public SortOptions SortType => SortOptions.Descending;

        public DescendingSortService(IProductsService productsService)
        {
            _productsService = productsService;
        }
        public async Task<List<Product>> SortAsync()
        {
            var products = await _productsService.GetAsync();
            return products.OrderByDescending(x => x.Name).ToList();
        }
    }
}
