using System.Collections.Generic;
using System.Linq;
using WooliesXTest.Data.Enums;
using WooliesXTest.Services.Interfaces;

namespace WooliesXTest.Services.Factory
{
    public class ProductSortFactory : IProductSortFactory
    {
        private readonly IEnumerable<IProductSortService> _productSortServices;

        public ProductSortFactory(IEnumerable<IProductSortService> productSortService)
        {
            _productSortServices = productSortService;
        }

        public IProductSortService GetSortService(SortOptions sortOption)
        {
            return _productSortServices.FirstOrDefault(i => i.SortType == sortOption);
        }
    }
}
