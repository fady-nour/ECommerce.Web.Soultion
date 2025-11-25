using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComerce.Shared.DTOS.IdentityDTOS
{
    public record UserDTO(string Email , string DisplayName ,string Token)
    {
    }
}
