using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Dtos;
using Dsw2025Ej15.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Dsw2025Ej15.Api.Controllers;

[ApiController]
[Route("api/orders/")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrdersManagementService _service;
    public OrdersController(IOrdersManagementService service)
    {
        _service = service;
    }

    [HttpGet]//Endpoint 7
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _service.GetOrders();
            return Ok(orders);
        }
        catch (EntityNotFoundException enfe)
        {
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

    [HttpPost]//Endpoint 6
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
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id}")]//8
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        try
        {
            // Especifica el tipo de retorno para evitar ambigüedad
            var order = await _service.GetOrderById(id); // Elimina el cast innecesario
            return Ok(order);
        }
        catch (EntityNotFoundException enfe)
        {
            return NotFound(enfe.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    //[HttpPut("{id}/status")]//9
    //public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderModel.OrderRequest request)
    //{
    //    try
    //    {
    //        var order = await _service.UpdateOrderStatus(id, request);
    //        return Ok(order);
    //    }
    //    catch (EntityNotFoundException enfe)
    //    {
    //        return NotFound(enfe.Message);
    //    }
    //    catch (ArgumentException ae)
    //    {
    //        return BadRequest(ae.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Problem(ex.Message);
    //    }
    //}
}