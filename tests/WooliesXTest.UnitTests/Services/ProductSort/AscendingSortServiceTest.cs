using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.Data.Enums;
using WooliesXTest.Data.Models;
using WooliesXTest.Services.Interfaces;
using WooliesXTest.Services.Services;
using WooliesXTest.Services.Services.ProductSort;
using Xunit;

namespace WooliesXTest.UnitTests.Services.ProductSort
{
    public class AscendingSortServiceTest
    {
        private List<Product> _products;
        private readonly IProductSortService _productSortService;
        private readonly IProductsService _productsService;

        public AscendingSortServiceTest()
        {
            _productsService = Substitute.For<IProductsService>();
            _productSortService = new AscendingSortService(_productsService);
            this.SetUp();
        }

        [Fact]
        public void SortType_ShouldReturnsAscendingOption()
        {
            const SortOptions expected = SortOptions.Ascending;

            var result = _productSortService.SortType;

            result.ShouldBe(expected);
        }

        [Fact]
        public async Task Sort_ShouldReturnsAscendingProducts()
        {
            _productsService.GetAsync().Returns(_products);

            var orderedProducts = await this._productSortService.SortAsync();

            orderedProducts.First().Name.ShouldBe("Product A");
        }

        private void SetUp()
        {
            _products = new List<Product> {
            new Product()
            {
                Name =  "Product B",
                Quantity =  10,
                Price =  10
            },
            new Product()
            {
                Name = "Product A",
                Quantity = 10,
                Price = 50
            }};
        }
    }
}
