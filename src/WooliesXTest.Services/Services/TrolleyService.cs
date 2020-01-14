using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.Data.Models;
using WooliesXTest.Data.ViewModels;

namespace WooliesXTest.Services.Services
{
    public interface ITrolleyService
    {
        Task<decimal> CalculateTrolleyTotal(TrolleyRequest trolley);
    }

    public class TrolleyService : ITrolleyService
    { 
        /// <summary>
        /// This method is an implementation of the WooliesX trolleyTotal end point
        /// </summary>
        /// <param name="trolleyRequest"></param>
        /// <returns></returns>
        public async Task<decimal> CalculateTrolleyTotal(TrolleyRequest trolleyRequest)
        {
            var applicableSpecials = new List<Special>();
            var trolleyProducts = trolleyRequest.Products;
            var trolleySpecials = trolleyRequest.Specials;
            var trolleyQuantities = trolleyRequest.Quantities;

            var products = (from p in trolleyProducts
                            join q in trolleyQuantities on p.Name equals q.Name
                            select new TrolleyProduct { Name = p.Name, Price = p.Price, Quantity = q.Quantity, QuantityLeft = q.Quantity }).ToList();

            // loop through all specials in the trolley request and compute the benefits
            // here benefit = total value of products without any special applied - applicable special value
            // we then find the special with highest benefit and use that for calculating the trolley total
            if (trolleySpecials != null && trolleySpecials.Any())
            {
                foreach (var special in trolleySpecials)
                {
                    var totalPrice = (from p in products
                                      join pq in special.Quantities on p.Name equals pq.Name
                                      group new { p, pq } by p.Name
                                            into productQtyGroup
                                      select productQtyGroup.Sum(x => x.p.Price * x.pq.Quantity)).Sum();

                    special.TotalBenefit = totalPrice - special.Total;
                }

                // apply specials recursively by reducing trolley product quantities
                // do this until no more specials can be applied
                var result = GetApplicableSpecials(products, trolleySpecials);

                if(result != null && result.Any())
                {
                    applicableSpecials.AddRange(result);
                }
            }

            return ((applicableSpecials?.Sum(x => x.Total) ?? 0) + products.Sum(p => p.QuantityLeft * p.Price));
        }

        private List<Special> GetApplicableSpecials(List<TrolleyProduct> products, List<Special> specials)
        {
            var applicableSpecials = new List<Special>();
            var selectedSpecials = new List<Special>();

            foreach (var special in specials.Where(x => x.TotalBenefit > 0))
            {
                var apply = true;
                foreach (var productQuantity in special.Quantities)
                {
                    if (products.All(x => x.Name != productQuantity.Name || (x.QuantityLeft < productQuantity.Quantity)))
                    {
                        apply = false;
                        break;
                    }
                }

                if (apply)
                {
                    applicableSpecials.Add(special);
                }
            }

            if (applicableSpecials.Any())
            {
                var selectedSpecial = applicableSpecials.OrderByDescending(x => x.TotalBenefit).FirstOrDefault();

                selectedSpecials.Add(selectedSpecial);

                foreach (var productQuantity in selectedSpecial?.Quantities)
                {
                    var product = products.FirstOrDefault(x => x.Name == productQuantity.Name);
                    if (product != null)
                    {
                        product.QuantityLeft = product.QuantityLeft - productQuantity.Quantity;
                    }
                }

                selectedSpecials.AddRange(GetApplicableSpecials(products, specials));
            }

            return selectedSpecials;
        }
    }
}
