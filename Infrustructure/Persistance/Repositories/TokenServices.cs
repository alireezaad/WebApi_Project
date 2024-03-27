using Application.Services_Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Infrustructure.Persistance.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class TokenServices : ITokenServices
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly ISecurityHelper _securityHelper;
        public TokenServices(ApiDbContext apiDbContext, ISecurityHelper securityHelper)
        {
            _apiDbContext = apiDbContext;
            _securityHelper = securityHelper;
        }
        public async Task DeleteRangeTokensAsync(int userId)
        {
            var allTokens = _apiDbContext.UserTokens.Where(token => token.UserId == userId).AsNoTracking().ToList();
            _apiDbContext.UserTokens.RemoveRange(allTokens);
            await _apiDbContext.SaveChangesAsync();
        }

        public async Task DeleteTokenAsync(Guid userTokenId)
        {
            _apiDbContext.UserTokens.Remove(new UserToken { Id = userTokenId });
            await _apiDbContext.SaveChangesAsync();
        }


        public async Task<UserToken> SaveTokenAsync(UserToken userTokens)
        {
            userTokens.Token = _securityHelper.QuickHash(userTokens.Token);
            userTokens.RefreshToken = _securityHelper.QuickHash(userTokens.RefreshToken);
            var createdTokens = _apiDbContext.Add(userTokens);
            await _apiDbContext.SaveChangesAsync();
            return createdTokens.Entity;
        }

        public bool CheckExistsToken(int userId, string token)
        {
            var userTokens = _apiDbContext.UserTokens.AsNoTracking().Where(token => token.UserId == userId).ToList();
            return userTokens.Any() && userTokens.Select(token => token.Token).Contains(_securityHelper.QuickHash(token));
        }
    }
}
