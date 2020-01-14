using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WooliesXTest.Api.Validators;
using WooliesXTest.Data.Configs;
using WooliesXTest.Data.Enums;
using WooliesXTest.Data.Models;
using WooliesXTest.Services.Interfaces;
using WooliesXTest.Services.Services;

namespace WooliesXTest.Api.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly ITrolleyService _trolleyService;
        private readonly IProductSortFactory _productSortFactory;
        private readonly ApiConfig _config;
        private readonly ILogger<AnswersController> _logger;

        public AnswersController(ITrolleyService trolleyService,
            IProductSortFactory productSortFactory,
            IOptions<ApiConfig> config,
            ILogger<AnswersController> logger)
        {
            _trolleyService = trolleyService;
            _productSortFactory = productSortFactory;
            _config = config.Value;
            _logger = logger;
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            try
            {
                var result = new User
                {
                    Name = "Aditi Dani",
                    Token = _config.ApiToken
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occured while processing GET /user.");
                throw;
            }
        }

        [HttpGet("sort")]
        public async Task<IActionResult> SortProducts(SortOptions sortOption)
        {
            if (sortOption == SortOptions.None)
            {
                return BadRequest("Invalid sort option");
            }

            try
            {
                _logger.LogInformation("SortProducts Requested:", sortOption);

                var productSortService = _productSortFactory.GetSortService(sortOption);
                var products = await productSortService.SortAsync();

                return Ok(products);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occured while processing GET /sort.");
                throw;
            }
        }

        [HttpPost("trolleytotal")]
        public async Task<IActionResult> GetTrolleyTotal([FromBody] TrolleyRequest trolleyRequest)
        {
            var validationResult = new TrolleyRequestValidator().Validate(trolleyRequest);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            try
            {
                _logger.LogInformation("GetTrolleyTotal Requested:", trolleyRequest);

                var trolleyTotal = await _trolleyService.CalculateTrolleyTotal(trolleyRequest);
                return Ok(trolleyTotal);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occured while calculating trolley total.");
                throw;
            }
        }
    }
}
