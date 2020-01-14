using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesXTest.Data.Models;
using WooliesXTest.Data.ViewModels;
using WooliesXTest.Services.Services;
using Xunit;

namespace WooliesXTest.UnitTests.Services
{
    public class TrolleyServiceTestShould
    {
        [Fact]
        public async Task CalculateLowestTrolleyTotal()
        {
            var trolleyRequest = new TrolleyRequest()
            {
                Products = new List<Product>
                {
                    new Product()
                    {
                        Name = "ProductA",
                        Price = 15.40150525M
                    }
                },
                Quantities = new List<ProductQuantity>
                {
                    new ProductQuantity()
                    {
                        Name = "ProductA",
                        Quantity = 9
                    }
                },
                Specials = new List<Special>
                {
                    new Special()
                    {
                        Quantities = new List<ProductQuantity>
                        {
                            new ProductQuantity()
                            {
                                Name = "ProductA",
                                Quantity = 5
                            }
                        },
                        Total = 50
                    },
                    new Special()
                    {
                        Quantities = new List<ProductQuantity>
                        {
                            new ProductQuantity()
                            {
                                Name = "ProductA",
                                Quantity = 3
                            }
                        },
                        Total = 35
                    }
                }
            };

            var result = await new TrolleyService().CalculateTrolleyTotal(trolleyRequest);
            result.ShouldBe(100.40150525M);
        }
    }
}
