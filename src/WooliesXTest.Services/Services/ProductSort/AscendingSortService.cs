using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.Data.Enums;
using WooliesXTest.Data.Models;
using WooliesXTest.Services.Interfaces;

namespace WooliesXTest.Services.Services.ProductSort
{
    public class AscendingSortService : IProductSortService
    {
        private readonly IProductsService _productsService;
        public SortOptions SortType => SortOptions.Ascending;

        public AscendingSortService(IProductsService productsService)
        {
            _productsService = productsService;
        }
        public async Task<List<Product>> SortAsync()
        {
            var products = await _productsService.GetAsync();
            return products?.OrderBy(x => x.Name).ToList();
        }
    }
}
