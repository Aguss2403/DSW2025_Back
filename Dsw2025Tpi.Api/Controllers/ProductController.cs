using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/products/")]
public class ProductController : ControllerBase
{
    private readonly ProductManagementService _service;
    public ProductController(ProductManagementService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody]ProductModel.Request request)
    {
        try
        {
            var product = await _service.AddProduct(request);
            return Ok(product);
        }
        catch (ArgumentException ae)
        {
            return BadRequest(ae.Message);
        }
        catch (ApplicationException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception)
        {
            return Problem("Se produjo un error al guardar el producto");
        }
    }
}
