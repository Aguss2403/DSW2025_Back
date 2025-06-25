using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
public class CustomerController : ControllerBase
{
    [HttpGet]
    [Route("api/customers/")]
    public IActionResult GetCustomers()
    {
        return Ok("List of customers");
    }
}
