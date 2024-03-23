using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IAuthenticationServices
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByPhonenumberAsync(string phonenumber);
        bool VerifyPassword(User user, string password);
        Task<UserToken> FindTokenWithRefreshTokenAsync(string hashedRefreshToken);
        Task<UserToken> GenerateTokenAsync(User user);
        Task GenerateSmsCodeAsync(string phoenumber);
        Task<SmsCode> VerifySmsCode(string phonenumber, string code);
    }
}
