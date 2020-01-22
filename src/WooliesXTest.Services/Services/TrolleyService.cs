using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WooliesXTest.Data.Models;
using WooliesXTest.Data.ViewModels;

namespace WooliesXTest.Services.Services
{
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
                            select new TrolleyProduct { Name = p.Name, Price = p.Price, Quantity = q.Quantity }).ToList();


            if (trolleySpecials != null && trolleySpecials.Any())
            {
                foreach (var special in trolleySpecials)
                {
                    var totalPrice = (from tp in products
                                      join sp in special.Quantities on tp.Name equals sp.Name
                                      group new { tp, sp } by tp.Name
                                            into productQtyGroup
                                      select productQtyGroup.Sum(x => x.tp.Price * x.sp.Quantity)).Sum();

                    special.TotalBenefit = totalPrice - special.Total;
                }

                var result = GetApplicableSpecials(products?.ToDictionary(x => x.Name), trolleySpecials);

                if (result != null && result.Any())
                {
                    applicableSpecials.AddRange(result);
                }
            }

            return ((applicableSpecials?.Sum(x => x.Total) ?? 0) + products.Sum(p => p.Quantity * p.Price));
        }

        private static List<Special> GetApplicableSpecials(IReadOnlyDictionary<string, TrolleyProduct> productDisDictionary, IEnumerable<Special> specials)
        {
            var applicableSpecials = new List<Special>();
            var allApplicableSpecials = new List<Special>();

            foreach (var special in specials.Where(x => x.TotalBenefit > 0))
            {
                var apply = true;

                foreach (var productQuantity in special.Quantities)
                {
                    bool isTrollyProductExist = productDisDictionary.TryGetValue(productQuantity.Name, out TrolleyProduct trolleyProduct);

                    if (isTrollyProductExist == false || trolleyProduct == null || trolleyProduct.Quantity < productQuantity.Quantity)
                    {
                        apply = false;
                        break;
                    }
                }

                if (apply)
                {
                    allApplicableSpecials.Add(special);
                }
            }

            if (allApplicableSpecials.Any())
            {
                var selectedSpecial = allApplicableSpecials.OrderByDescending(x => x.TotalBenefit).FirstOrDefault();

                applicableSpecials.Add(selectedSpecial);

                ApplySpecialToTrolley(productDisDictionary, selectedSpecial);

                applicableSpecials.AddRange(GetApplicableSpecials(productDisDictionary, specials));
            }

            return applicableSpecials;
        }

        private static void ApplySpecialToTrolley(IReadOnlyDictionary<string, TrolleyProduct> productDisDictionary, Special special)
        {
            foreach (var productQuantity in special?.Quantities)
            {
                productDisDictionary.TryGetValue(productQuantity.Name, out var trolleyProduct);

                if (trolleyProduct != null)
                {
                    trolleyProduct.Quantity -= productQuantity.Quantity;
                }
            }
        }
    }
}
