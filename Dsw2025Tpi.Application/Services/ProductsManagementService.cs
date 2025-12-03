using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions; // Asegúrate de que este namespace es correcto
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System.Text.RegularExpressions; // Para Regex

namespace Dsw2025Tpi.Application.Services
{
    public class ProductsManagementService : IProductsManagementService
    {
        private readonly IRepository _repository;

        public ProductsManagementService(IRepository repository)
        {
            _repository = repository;
        }

        public void ValidateRequest(ProductModel.ProductRequest request)
        {
            // CORREGIDO: Usamos ValidationException en lugar de ApplicationException
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new ValidationException("El SKU es requerido.", 3003);

            if (!Regex.IsMatch(request.Sku, "^SKU-\\d{4}$"))
                throw new InvalidFormatSKUException(); // Este ya tiene su código 3000 fijo

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("El nombre es requerido.", 3004);

            if (request.CurrentUnitPrice <= 0)
                throw new ValidationException("El precio unitario debe ser mayor a 0.", 3001);

            if (request.StockQuantity < 0)
                throw new ValidationException("La cantidad en stock no puede ser negativa.", 3002);
        }

        private ProductModel.ProductResponse MapToResponse(Product product)
        {
            return new ProductModel.ProductResponse(
                product.Id,
                product.Sku,
                product.InternalCode,
                product.Name,
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive);
        }

        public async Task<ProductModel.ProductResponse?> GetProductById(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            
            if (product == null)
                throw new EntityNotFoundException($"No existe el producto con Id: {id}");

            return MapToResponse(product);
        }

        public async Task<ProductModel.ResponsePagination?> GetProducts(ProductModel.FilterProduct? request = null)
        {
            request ??= new ProductModel.FilterProduct(); 

            var isActive = request.Status == "enabled" ? (bool?)true :
                           request.Status == "disabled" ? (bool?)false : null;

            var activeProducts = await _repository.GetFiltered<Product>(p =>
                (isActive == null || p.IsActive == isActive) &&
                (string.IsNullOrEmpty(request.Search) || p.Name.Contains(request.Search))
            );

            // CORREGIDO: Usar NoContentException para listas vacías (Devuelve 204)
            if (activeProducts is null || !activeProducts.Any())
                throw new NoContentException("No hay productos cargados");

            var products = activeProducts
                .Select(p => new ProductModel.ProductResponse(
                    p.Id,
                    p.Sku,
                    p.InternalCode,
                    p.Name,
                    p.Description,
                    p.CurrentUnitPrice,
                    p.StockQuantity,
                    p.IsActive))
                .OrderBy(p => p.Sku)
                .Skip(((request.PageNumber ?? 1) - 1) * (request.PageSize ?? activeProducts.Count()))
                .Take(request.PageSize ?? activeProducts.Count())
                .ToList();

            return new ProductModel.ResponsePagination(products, activeProducts.Count());
        }

        public async Task<ProductModel.ProductResponse> AddProduct(ProductModel.ProductRequest request)
        {
            ValidateRequest(request);

            var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
            if (exist != null)
                throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}"); // Devuelve 409

            var product = new Product(request.Sku,
                request.InternalCode,
                request.Name,
                request.Description,
                request.CurrentUnitPrice,
                request.StockQuantity);

            await _repository.Add(product);

            return MapToResponse(product);
        }

        public async Task<ProductModel.ProductResponse?> Update(Guid id, ProductModel.ProductRequest request)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null)
                throw new EntityNotFoundException($"No se encontró un producto con el ID {id}");

            ValidateRequest(request);

            // Validación de duplicado al actualizar (solo si el SKU cambió)
            if (request.Sku != product.Sku)
            {
                var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
                if (exist != null)
                    throw new DuplicatedEntityException($"Ya existe un producto con el Sku {request.Sku}");
            }

            product.Sku = request.Sku;
            product.InternalCode = request.InternalCode;
            product.Name = request.Name;
            product.Description = request.Description;
            product.CurrentUnitPrice = request.CurrentUnitPrice;
            product.StockQuantity = request.StockQuantity;

            var updatedProduct = await _repository.Update(product);

            return MapToResponse(product);
        }

        public async Task<ProductModel.ProductResponse?> ToggleStatus(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null)
                throw new EntityNotFoundException($"No se encontró un producto con el ID {id}");

            // Lógica de toggle (invertir estado actual)
            product.IsActive = !product.IsActive; 

            var updatedProduct = await _repository.Update(product);

            return MapToResponse(product);
        }
    }
}