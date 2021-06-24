using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class PagingModel<TResult> where TResult : class
    {
        public List<TResult> Data { get; set; }
        public long TotalRecord { get; set; }
    }
}
