using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComerce.Shared.CommonResult
{
    public class Error
    {

        public string Code { get;}
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }
        public static Error Failure(string code="General Failure", string description= "General Failure Occured !")
        {
            return new Error(code ,description,ErrorType.Failure);
        }
        public static Error Validaion(string code="Validation Error", string description= "Validation Error Occured !")
        {
            return new Error(code ,description,ErrorType.Validation);
        }
        public static Error NotFound(string code="Not Found", string description= "Error 404 Not FoundGeneral   !")
        {
            return new Error(code ,description,ErrorType.NotFound);
        }
        public static Error Unauthorized(string code= "Unauthorized Error ", string description= "Unauthorized  Occured !")
        {
            return new Error(code ,description,ErrorType.Unauthorized);
        }
        public static Error Forbidden(string code="Forbidden Error", string description= "Forbidden Error Ocuured !")
        {
            return new Error(code ,description,ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string code= "InvalidCredentials Error", string description= "InvalidCredentials Error Occured !")
        {
            return new Error(code ,description,ErrorType.InvalidCredentails);
        }

        //No error  -error one - more than
        
    }
}
