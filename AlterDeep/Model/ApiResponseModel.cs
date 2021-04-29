using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlterDeep.Model
{
    public class ApiResponseModel<T>
    {
        public T Data { get; set; }
        public bool IsSucces { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
