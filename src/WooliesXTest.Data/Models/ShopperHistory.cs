using System.Collections.Generic;

namespace WooliesXTest.Data.Models
{
    public class ShopperHistory
    {
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }
    }
}
