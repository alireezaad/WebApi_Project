using Application.Services;
using Application.Services_Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.EntitiesConfigurations
{
    public class UserTokenConfigs : IEntityTypeConfiguration<UserToken>
    {
        private readonly ISecurityHelper _securityHelper;

        public UserTokenConfigs()
        {
        }

        public UserTokenConfigs(ISecurityHelper securityHelper)
        {
            _securityHelper = securityHelper;
        }
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            //var ddd = ;
            //builder.Property(t => t.RefreshTokenExpired).HasConversion(y => y, y=> y. > DateTime.Now ? false : true);
            //builder.Property(t => t.Token)
            //    .HasConversion(t => _securityHelper.QuickHash(t), t => t.ToString());

            //builder.Property(t => t.RefreshToken)
            //    .HasConversion(t => _securityHelper.QuickHash(t), t => t.ToString());
        }
    }
}
