using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Services
{
    public interface IOrdersManagementService
    {
        Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request);
    }
}