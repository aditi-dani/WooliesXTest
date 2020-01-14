using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WooliesXTest.Api;
using WooliesXTest.Data.Models;
using WooliesXTest.Data.ViewModels;
using Xunit;

namespace WooliesXTest.IntegrationTests
{
    public class AnswersApiTest : IClassFixture<TestFixture<Startup>>
    {
        private readonly HttpClient _testClient;

        public AnswersApiTest(TestFixture<Startup> fixture)
        {
            _testClient = fixture.Client;
        }

        [Fact]
        public async Task GetUser_ReturnsValidUserName()
        {
            var requestUrl = "/api/answers/user";

            var response = await _testClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadAsAsync<User>();

            user.Name.ShouldBe("Aditi Dani");
        }

        [Fact]
        public async Task ProductSort_WithInvalidSortOption_ReturnsBadRequest()
        {
            var requestUrl = "/api/answers/sort";

            var response = await _testClient.GetAsync(requestUrl);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ProductSort_WithValidSortOption_ReturnsOk()
        {
            var requestUrl = "/api/answers/sort?sortOption=Low";

            var response = await _testClient.GetAsync(requestUrl);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TrolleyTotal_WithValidTrolleyRequest_ReturnsLowestTrolleyTotal()
        {
            var requestUrl = "/api/answers/trolleytotal";

            var requestBody = new StringContent(JsonConvert.SerializeObject(new TrolleyRequest()
            {
                Products = new List<Product>
                {
                    new Product()
                    {
                        Name = "ProductA",
                        Price = 15.20075262887435M
                    }
                },
                Quantities = new List<ProductQuantity>
                {
                    new ProductQuantity()
                    {
                    Name = "ProductA",
                    Quantity = 15
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
                                Quantity = 8
                            }
                        },
                        Total = 60
                    }
                }
            }), Encoding.UTF8, "application/json");

            var response = await _testClient.PostAsync(requestUrl, requestBody);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var trolleyTotal = await response.Content.ReadAsAsync<decimal>();
            trolleyTotal.ShouldBe(140.40150525774870M);

        }

        [Fact]
        public async Task TrolleyTotal_WithInvalidTrolleyRequest_ReturnsBadRequest()
        {
            var requestUrl = "/api/answers/trolleytotal";

            var requestBody = new StringContent(JsonConvert.SerializeObject(new TrolleyRequest()
            {
            }), Encoding.UTF8, "application/json");

            var response = await _testClient.PostAsync(requestUrl, requestBody);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

    }
}
