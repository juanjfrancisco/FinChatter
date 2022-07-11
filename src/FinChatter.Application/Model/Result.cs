using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Application.Model
{
    public  class Result <T>
    {        
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
    }
}
