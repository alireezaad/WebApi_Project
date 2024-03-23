using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ITokenServices
    {
        Task<UserToken> SaveTokenAsync(UserToken userTokens);
        Task DeleteTokenAsync(Guid userTokenId);
    }
}
