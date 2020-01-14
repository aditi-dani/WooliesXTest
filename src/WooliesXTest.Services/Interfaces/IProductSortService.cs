using System.Collections.Generic;
using System.Threading.Tasks;
using WooliesXTest.Data.Enums;
using WooliesXTest.Data.Models;

namespace WooliesXTest.Services.Interfaces
{
    public interface IProductSortService
    {
        SortOptions SortType { get; }
      Task<List<Product>> SortAsync();
    }
}
