using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Answer
{
    public class AnswerPagingModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int UserId { get; set; }
    }
}
