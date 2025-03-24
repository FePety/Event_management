using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_management.Core.Models
{
    public class ApiResponse<T> : BaseResponse
    {
        public T Data { get; set; }

        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}
