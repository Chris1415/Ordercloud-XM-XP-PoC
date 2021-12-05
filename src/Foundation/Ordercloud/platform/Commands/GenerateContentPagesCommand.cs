using BasicCompany.Foundation.Products.Ordercloud.Services;
using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Commands
{
    public class GenerateContentPagesCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            var _service = DependencyResolver.Current.GetService<IContentPagesService>();
            var success = false;

            Task.Run(() =>
            {
                using (new DatabaseSwitcher(Factory.GetDatabase("master")))
                {
                    success = _service.Generate();
                }
            });

            SheerResponse.Alert($"Generating ...");
        }
    }
}