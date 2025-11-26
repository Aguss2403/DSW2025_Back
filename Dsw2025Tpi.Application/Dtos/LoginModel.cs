using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos;

public record RequestLoginModel(string Username, string Password);

public record ResponseLoginModel(
     string Token,
     Guid Id,
     string Username,
     string Email,
     string Role,
     Guid CustomerId,
     string FirstName,
     string LastName,
     string Address,
     string PhoneNumber
     );

