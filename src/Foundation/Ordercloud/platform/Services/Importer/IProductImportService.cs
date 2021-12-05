using OrderCloud.SDK;

namespace BasicCompany.Foundation.Products.Ordercloud.Services.Importer
{
    public interface IProductImportService
    {
        bool Import<T>() where T : Product;
    }
}
