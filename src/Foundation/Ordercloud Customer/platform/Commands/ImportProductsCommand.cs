using BasicCompany.Foundation.Products.Ordercloud.Customer.Models;
using BasicCompany.Foundation.Products.Ordercloud.Customer.Services.Importer;
using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Customer.Commands
{
    public class ImportProductsCommand : Ordercloud.Commands.ImportProductsCommand
    {
        protected override IProductImportService InjectService()
        {
            return DependencyResolver.Current.GetService<ICustomProductImportService>();
        }

        protected override bool ExecuteImport()
        {
            var _importer = InjectService();
            return _importer.Import<MyProduct>();
        }
    }
}