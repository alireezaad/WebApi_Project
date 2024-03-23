using Domain.Entities;
using Infrustructure.Persistance.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.DbContexts
{
    public class ApiDbContext : DbContext
    {
        public DbSet<TaskEntity> TaskEntities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<SmsCode> smsCodes { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfigs());
            builder.ApplyConfiguration(new TaskEntityConfigs());
            builder.ApplyConfiguration(new UserTokenConfigs());
        }
    }
}
