using Microsoft.AspNetCore.Mvc;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Ej15.Api.Controllers;

[ApiController]
[Route("api/orders/")]
public class OrderController : ControllerBase
{
    private readonly OrdersManagementService _service;

    public OrderController(OrdersManagementService service)
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
            //return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while creating the order: " + ex.Message);
        }
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateOrder([FromBody]OrderModel.Request request)
    //{
    //    try
    //    {
    //        if (request == null)
    //        {
    //            return BadRequest("The request body cannot be null.");
    //        }

    //        var order = await _service.AddOrder(request);
    //        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    //    }
    //    catch (ArgumentException ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Problem("An error occurred while creating the order: " + ex.Message);
    //    }
    //}


}