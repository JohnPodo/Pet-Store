using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Models.Responses
{
    public class DataResponse<T> : BaseResponse
    {
        public DataResponse(bool success, string message,T data) : base(success, message)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
