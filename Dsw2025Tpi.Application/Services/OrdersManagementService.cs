using Dsw2025Ej15.Application.Exceptions;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services;

public class OrdersManagementService
{
    private readonly IRepository _repository;
    public OrdersManagementService(IRepository repository)
    {
        _repository = repository;
    }

    //public async Task<ProductModel.Response> AddOrder(ProductModel.Request request)
    //{
    //    if (string.IsNullOrWhiteSpace(request.Sku) ||
    //        string.IsNullOrWhiteSpace(request.Name) ||
    //        request.CurrentUnitPrice <= 0 ||
    //        request.StockQuantity < 0)
    //    {
    //        throw new ArgumentException("Los datos del producto son inválidos.");
    //    }

    //    var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
    //    if (exist != null) throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");

    //    var product = new Product(request.Sku,
    //        request.InternalCode,
    //        request.Name,
    //        request.Description,
    //        request.CurrentUnitPrice,
    //        request.StockQuantity);

    //    await _repository.Add(product);

    //    //return MapToResponse(product);
    //}

}
