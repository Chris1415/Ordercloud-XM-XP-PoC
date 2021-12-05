using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using OrderCloud.SDK;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Commands
{
    public class ImportProductsCommand : Command
    {
        protected virtual IProductImportService InjectService()
        {
            return DependencyResolver.Current.GetService<IProductImportService>();
        }

        protected virtual bool ExecuteImport()
        {
            var _importer = InjectService();
            return _importer.Import<Product>();
        }

        public override void Execute(CommandContext context)
        {
            var success = false;

            Task.Run(() =>
            {
                using (new DatabaseSwitcher(Factory.GetDatabase("master")))
                {
                    success = ExecuteImport();
                }
            });

            SheerResponse.Alert($"Importing ...");
        }
    }
}