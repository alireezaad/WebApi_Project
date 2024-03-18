using Domain.Entities;
using Domain.IRepositories;
using Infrustructure.Persistance.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApiDbContext dbCOntext) : base(dbCOntext)
        {
        }
    }
}
