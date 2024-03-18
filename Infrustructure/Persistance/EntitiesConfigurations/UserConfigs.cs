using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.EntitiesConfigurations
{
    public class UserConfigs : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasQueryFilter(u => u.IsActive);
            builder.Property(u => u.IsActive).HasDefaultValue(true);
        }
    }
}
