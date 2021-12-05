﻿using BasicCompany.Foundation.Products.Ordercloud.Services.Importer;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BasicCompany.Foundation.Products.Ordercloud.Commands
{
    public class ImportSuppliersCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            ISupplierImportService _importer = DependencyResolver.Current.GetService<ISupplierImportService>();
            var success = false;

            Task.Run(() =>
            {
                using (new DatabaseSwitcher(Factory.GetDatabase("master")))
                {
                    success = _importer.Import();
                }
            });

            SheerResponse.Alert($"Importing ...");
        }
    }
}