using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComerce.Shared.CommonResult
{
    public enum ErrorType
    {
            Failure = 0,
            Validation  =1,
            NotFound=2,
            Unauthorized =3,
            Forbidden =4,
            InvalidCredentails = 5

    }
}
