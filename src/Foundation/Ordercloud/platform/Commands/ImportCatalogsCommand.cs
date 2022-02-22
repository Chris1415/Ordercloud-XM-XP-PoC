using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Tasks;
using Sitecore.Web.UI.Sheer;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Commands
{
    public class ImportCatalogsCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            ImportCatalogs();
            SheerResponse.Alert($"Importing ...");
        }

        public void ExecuteScheduled(Item[] items, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            ImportCatalogs();
        }

        private void ImportCatalogs()
        {
            ICatalogImportService _importer = DependencyResolver.Current.GetService<ICatalogImportService>();
            var success = false;

            System.Threading.Tasks.Task.Run(() =>
            {
                using (new DatabaseSwitcher(Factory.GetDatabase("master")))
                {
                    success = _importer.Import();
                }
            });
        }
    }
}