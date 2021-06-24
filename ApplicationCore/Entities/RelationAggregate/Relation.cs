using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities.RelationAggregate
{
    public class Relation : BaseEntity, IAggregateRoot
    {
        public int UserId { get; set; }
        public int LinkToId { get; set; }
        public virtual User User { get; set; }
    }
}
