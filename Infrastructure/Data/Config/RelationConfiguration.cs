using ApplicationCore.Entities.RelationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Config
{
    public class RelationConfiguration : IEntityTypeConfiguration<Relation>
    {
        public void Configure(EntityTypeBuilder<Relation> builder)
        {
            builder.ToTable(nameof(Relation));

        }
    }
}
