using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using WooliesXTest.Data.Enums;
using WooliesXTest.Data.Models;
using WooliesXTest.Services.Interfaces;
using WooliesXTest.Services.Services;
using WooliesXTest.Services.Services.ProductSort;
using Xunit;

namespace WooliesXTest.UnitTests.Services.ProductSort
{
    public class RecommendedSortServiceTest
    {
        private List<Product> _products;
        private readonly IProductSortService _productSortService;
        private readonly IProductsService _productsService;
        private readonly IShopperHistoryService _shopperHistoryService;

        public RecommendedSortServiceTest()
        {
            _shopperHistoryService = Substitute.For<IShopperHistoryService>();
            _productsService = Substitute.For<IProductsService>();
            _productSortService = new RecommendedSortService(_productsService, _shopperHistoryService);

            this.SetUp();
        }

        [Fact]
        public void SortType_ShouldReturnsRecommendedOption()
        {
            const SortOptions expected = SortOptions.Recommended;

            var result = _productSortService.SortType;

            result.ShouldBe(expected);
        }

        [Fact]
        public async Task Sort_ShouldReturnRecommendedSortedProducts()
        {
            _productsService.GetAsync().Returns(_products);
            _shopperHistoryService.GetAsync().Returns(new List<ShopperHistory>()
            {
                new ShopperHistory()
                {
                    CustomerId = 1,
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Product C",
                            Quantity = 50
                        },
                        new Product()
                        {
                            Name = "Product A",
                            Quantity = 10
                        },
                        new Product()
                        {
                            Name = "Product B",
                            Quantity = 5
                        }
                    }
                }
            });

            var orderedProducts = await this._productSortService.SortAsync();

            orderedProducts.First().Name.ShouldBe("Product C");
            orderedProducts.Last().Name.ShouldBe("Product D");
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
                },
                new Product()
                {
                    Name = "Product C",
                    Quantity = 10,
                    Price = 50
                },
                new Product()
                {
                    Name = "Product D",
                    Quantity = 7
                }
            };
        }
    }
}
