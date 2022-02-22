using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicCompany.Feature.Products.Models.Webhooks
{
    public class ProductWebhookModel
    {
        public string Route { get; set; }
        public Routeparams RouteParams { get; set; }
        public string Verb { get; set; }
        public DateTime Date { get; set; }
        public string LogID { get; set; }
        public string UserToken { get; set; }
        public Request Request { get; set; }
        public Response Response { get; set; }
        public object[] ConfigData { get; set; }
    }

    public class Routeparams
    {
    }

    public class Request
    {
        public Body Body { get; set; }
        public object Headers { get; set; }
    }

    public class Body
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }

    public class Response
    {
        public Body1 Body { get; set; }
        public object Headers { get; set; }
    }

    public class Body1
    {
        public string OwnerID { get; set; }
        public object DefaultPriceScheduleID { get; set; }
        public bool AutoForward { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public object Description { get; set; }
        public int QuantityMultiplier { get; set; }
        public object ShipWeight { get; set; }
        public object ShipHeight { get; set; }
        public object ShipWidth { get; set; }
        public object ShipLength { get; set; }
        public bool Active { get; set; }
        public int SpecCount { get; set; }
        public int VariantCount { get; set; }
        public object ShipFromAddressID { get; set; }
        public object Inventory { get; set; }
        public object DefaultSupplierID { get; set; }
        public bool AllSuppliersCanSell { get; set; }
        public object xp { get; set; }
    }
}