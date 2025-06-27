using Dsw2025Ej15.Application.Exceptions;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/products/")]
public class ProductController : ControllerBase
{
    private readonly IProductsManagementService _service;
    public ProductController(IProductsManagementService service)
    {
        _service = service;
    }

    [HttpGet()]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _service.GetProducts();
        if (products == null || !products.Any()) return NoContent();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductBySku(Guid id)
    {
        var product = await _service.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody]ProductModel.Request request)
    {
        try
        {
            var product = await _service.AddProduct(request);
            return Created("/product",product);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (System.ApplicationException de)
        {
            return Conflict(de.Message);
        }
        catch (DuplicatedEntityException de)
        {
            return BadRequest(de.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al guardar el producto");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductModel.Request request)
    {
        try
        {
            var updatedProduct = await _service.Update(id, request);
            if (updatedProduct == null) throw new EntityNotFoundException($"No se encontró un producto con el ID {id}");
            return Ok(updatedProduct);
        }
        catch (EntityNotFoundException ef)
        {
            return NotFound(ef.Message);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (DuplicatedEntityException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar el producto");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ToggleProductStatus(Guid id)
    {
        try
        {
            var updatedProduct = await _service.ToggleStatus(id);
            if (updatedProduct == null) throw new EntityNotFoundException($"No se encontró un producto con el ID {id}");
            return Ok(updatedProduct);
        }
        catch (EntityNotFoundException ef)
        {
            return NotFound(ef.Message);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al actualizar el estado del producto");
        }
    }






}
