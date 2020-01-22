using System.Threading.Tasks;
using WooliesXTest.Data.Models;

namespace WooliesXTest.Services.Services
{
    public interface ITrolleyService
    {
        Task<decimal> CalculateTrolleyTotal(TrolleyRequest trolley);
    }
}