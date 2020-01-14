using WooliesXTest.Data.Enums;

namespace WooliesXTest.Services.Interfaces
{
    public interface IProductSortFactory
    {
        IProductSortService GetSortService(SortOptions sortOption);
    }
}