using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Services
{
    public interface IProductsManagementService
    {
        Task<ProductModel.Response> AddProduct(ProductModel.Request request);
        Task<ProductModel.Response?> GetProductById(Guid id);
        Task<IEnumerable<ProductModel.Response>?> GetProducts();
        Task<ProductModel.Response?> ToggleStatus(Guid id);
        Task<ProductModel.Response?> Update(Guid id, ProductModel.Request request);
    }
}