using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Account
{
    public class AccountPagingModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
