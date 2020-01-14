using System.Collections.Generic;
using WooliesXTest.Data.ViewModels;

namespace WooliesXTest.Data.Models
{
    public class TrolleyRequest
    {
        public List<Product> Products { get; set; }
        public List<Special> Specials { get; set; }
        public List<ProductQuantity> Quantities { get; set; }
    }
}
