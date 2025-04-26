using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; } 
        public string Message { get; set; }

        // Constructor para éxito
        public ApiResponse(T data)
        {
            Success = true;
            Data = data;
            Message = "Operación exitosa";
        }

        // Constructor para error
        public ApiResponse(string message)
        {
            Success = false;
            Data = default;
            Message = message;
        }
    }

}
