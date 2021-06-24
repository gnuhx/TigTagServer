using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class LogicResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        //Status code for computer
        public int? ErrorStatusCode { get; set; }
    }
}
