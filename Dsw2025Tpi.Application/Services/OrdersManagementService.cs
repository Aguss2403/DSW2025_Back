using Dsw2025Ej15.Application.Exceptions;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services;

public class OrdersManagementService : IOrdersManagementService
{
    private readonly IRepository _repository;


    public OrdersManagementService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderModel.OrderResponse> AddOrder(OrderModel.OrderRequest request)
    {
        var customer = await _repository.GetById<Customer>(request.CustomerId);
        if (customer == null)
            throw new EntityNotFoundException($"No se encontró el cliente con ID {request.CustomerId}");

        var orderItems = new List<OrderItem>();
        foreach (var item in request.OrderItems)
        {
            var product = await _repository.GetById<Product>(item.ProductId);
            if (product == null)
                throw new EntityNotFoundException($"No se encontró el producto con ID {item.ProductId}");

            if (!product.HasSufficientStock(item.Quantity))
                throw new InvalidOperationException($"Stock insuficiente para el producto {product.Name}.");
            product.DecreaseStock(item.Quantity);
            
            await _repository.Update(product);

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = item.Quantity,
                UnitPrice = product.CurrentUnitPrice
            };
            orderItem.SubTotal = orderItem.CalculateSubTotal();
            orderItems.Add(orderItem);
        }

        var order = new Order(
            request.ShippingAddress,
            request.BillingAddress,
            request.Notes,
            orderItems,
            request.CustomerId
        );

        await _repository.Add(order);

        var response = new OrderModel.OrderResponse(
            order.Id,
            order.CustomerId,
            order.ShippingAddress,
            order.BillingAddress,
            order.Notes,
            orderItems.Select(oi => new OrderModel.OrderItemResponse(
                oi.ProductId,
                oi.Product.Name,
                oi.UnitPrice,
                oi.Quantity,
                oi.SubTotal
            )).ToList(),
            order.Status.ToString()
        );

        return response;
    }

    public async Task<List<OrderModel.OrderResponse>?> GetOrders()
    {
        var orders = await _repository.GetAll<Order>(include: new[] { "OrderItems", "OrderItems.Product" });
        if (orders == null || !orders.Any())
            throw new EntityNotFoundException("No se encontraron órdenes.");

        return orders.Select(o => new OrderModel.OrderResponse(
        o.Id,
        o.CustomerId,
        o.ShippingAddress,
        o.BillingAddress,
        o.Notes,
        o.OrderItems.Select(oi => new OrderModel.OrderItemResponse(
            oi.ProductId,
            oi.Product.Name,
            oi.UnitPrice,
            oi.Quantity,
            oi.SubTotal
        )).ToList(), o.Status.ToString())).ToList();    
    }
}