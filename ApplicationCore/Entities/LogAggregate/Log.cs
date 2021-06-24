using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.LogAggregate
{
    public class Log : BaseEntity, IAggregateRoot
    {
        public string Action { get; set; }
        public bool Status { get; set; }
    }
}
