using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Ej15.Application.Exceptions;

namespace Dsw2025Ej15.Api.Controllers;

[ApiController]
[Route("api/orders/")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersManagementService _service;


    public OrdersController(IOrdersManagementService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        // Implementación real pendiente
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderModel.OrderRequest request)
    {
        try
        {
            var order = await _service.AddOrder(request);
            return Created("api/order", order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException io)
        {
            return BadRequest(io.Message);
        }
        catch (EntityNotFoundException enfe)
        {
            return NotFound(enfe.Message);
        }
        catch (DuplicatedEntityException de)
        {
            return Conflict(de.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}