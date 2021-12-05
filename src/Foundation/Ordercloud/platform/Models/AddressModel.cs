using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicCompany.Foundation.Products.Ordercloud.Models
{
    public class AddressModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string CompanyName { get; set; }
    }
}