using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Customer.Models
{
    public class MyProduct : Product<ProductXp>
    {
        public MyProduct()
        {
            xp = new ProductXp();
        }
    }
}