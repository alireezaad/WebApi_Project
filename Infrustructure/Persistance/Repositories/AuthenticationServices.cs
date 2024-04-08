using Application.Models.TokenModels;
using Application.Services;
using Domain.Entities;
using Domain.IRepositories;
using Infrustructure.Identity.JWT;
using Infrustructure.Persistance.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly ApiDbContext _dbContext;
        //private readonly SecurityHelper _securityHelper;
        private readonly JwtGenerator _jwtGenerator;
        private readonly SmsService _msService;
        public AuthenticationServices(ApiDbContext dbContext,/* SecurityHelper securityHelper,*/ JwtGenerator jwtGenerator, SmsService smsService)
        {
            _dbContext = dbContext;
            _jwtGenerator = jwtGenerator;
            //_securityHelper = securityHelper;
            _msService = smsService;
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
                throw new ArgumentException("Email is invalid");

            return user;
        }

        public async Task<User> FindByPhonenumberAsync(string phonenumber)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Phonenumber == phonenumber);
            return user;
        }

        public bool VerifyPassword(User user, string password)
        {
            return user != null && user.Password == password;
        }

        public async Task<UserToken> GenerateTokenAsync(User user)
        {
            return await _jwtGenerator.GeneratorAsync(user);
        }

        public async Task<UserToken> FindTokenWithRefreshTokenAsync(string hashedRefreshToken)
        {
            return await _dbContext.UserTokens.AsNoTracking().Include(u => u.User).SingleOrDefaultAsync(s => s.RefreshToken == hashedRefreshToken);
        }

        public async Task GenerateSmsCodeAsync(string phonenumber)
        {
            var smsCode = new SmsCode
            {
                Id = Guid.NewGuid(),
                Code = new Random().Next(1000, 9999).ToString(),
                RequestCount = 0,
                IsUsed = false,
                Phonenumber = phonenumber,
                GenerateDate = DateTime.Now,
            };
            _dbContext.smsCodes.Add(smsCode);
            await _dbContext.SaveChangesAsync();
            _msService.SendAsync(smsCode.Phonenumber, smsCode.Code);
        }

        public async Task<SmsCode> VerifySmsCode(string phonenumber, string code)
        {
            return await _dbContext.smsCodes.FirstOrDefaultAsync(s => s.Phonenumber.Equals(phonenumber) && s.Code.Equals(code));
        }

    }
}
