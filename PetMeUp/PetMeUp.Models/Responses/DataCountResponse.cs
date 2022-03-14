using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMeUp.Models.Responses
{
    public class DataCountResponse<T> : DataResponse<T>
    {
        public DataCountResponse(bool success, string message, T data,int totalCount) : base(success, message, data)
        {
            TotalCount = totalCount;
        }

        public int? TotalCount { get; set; }

    }
}
