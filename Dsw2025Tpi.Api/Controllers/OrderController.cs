using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Ej15.Api.Controllers;

[ApiController]
[Route("api/orders/")]
public class OrderController : ControllerBase
{
    //[HttpPost]
    //public async Task<IActionResult> CreateOrder([FromBody] OrderModel.Request request)
    //{
    //    try
    //    {
    //        // Assuming _service is injected and available
    //        var order = await _service.CreateOrder(request);
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
