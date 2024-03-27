using Application.Models.TokenModels;
using Application.Models.UserModels;
using Application.Services;
using Application.Services_Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AuthUseCases
{
    public class AuthenticationUC
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly ITokenServices _tokenServices;
        private readonly IServiceWrapper _serviceWrapper;
        private readonly ISecurityHelper _securityHelper;
        private readonly IMapper _mapper;
        public AuthenticationUC(IAuthenticationServices authenticationServices, ISecurityHelper securityHelper, IMapper mapper, IServiceWrapper serviceWrapper, ITokenServices tokenServices)
        {
            _authenticationServices = authenticationServices;
            _securityHelper = securityHelper;
            _mapper = mapper;
            _serviceWrapper = serviceWrapper;
            _tokenServices = tokenServices;
        }
        public async Task<TokenGenerateModel> AuthenticateWithPasswordAsync(UserAuthorizeModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password)) throw new ArgumentNullException("Email or password is null!");
                var user = await _authenticationServices.FindByEmailAsync(model.Email);
                if (_authenticationServices.VerifyPassword(user, model.Password))
                {
                    var userToken = await _authenticationServices.GenerateTokenAsync(user);
                    await _tokenServices.SaveTokenAsync(userToken);
                    return _mapper.Map<TokenGenerateModel>(userToken);

                }
                else
                    return new TokenGenerateModel() { };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<AuthenticateResultModel> AuthenticateWithRefreshToken(string refreshToken)
        {
            try
            {
                var authModel = new AuthenticateResultModel();
                var userToken = await _authenticationServices.FindTokenWithRefreshTokenAsync(_securityHelper.QuickHash(refreshToken));
                if (userToken == null || userToken.User == null)
                {
                    authModel.IsSuccess = false;
                    authModel.Message = "invalid token!";
                    return authModel;
                }

                if (userToken.RefreshTokenExpiration < DateTime.Now)
                {
                    authModel.IsSuccess = false;
                    authModel.Message = "refreshToken expired! authenticate again with your cridentials!";
                    return authModel;
                }
                // TODO redirect to login 

                var newUserToken = await _authenticationServices.GenerateTokenAsync(userToken.User);
                if (newUserToken == null)
                {
                    authModel.IsSuccess = false;
                    authModel.Message = "Authenricate failed!";
                    return authModel;
                }
                authModel.Tokens = _mapper.Map<TokenGenerateModel>(newUserToken); 
                authModel.RefreshTokenExpiration = userToken.RefreshTokenExpiration;
                authModel.IsSuccess = true;
                await _tokenServices.SaveTokenAsync(newUserToken);
                await _tokenServices.DeleteTokenAsync(userToken.Id);
                return authModel;


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GenerateCode(string phonenumber)
        {
            await _authenticationServices.GenerateSmsCodeAsync(phonenumber);
        }

        public async Task<AuthenticateResultModel> AuthenticateWithPhonenumber(string phonenumber, string code)
        {
            try
            {
                var authModel = new AuthenticateResultModel();
                var smsCode = _authenticationServices.VerifySmsCode(phonenumber, code).Result;
                if (smsCode == null)
                {
                    authModel.IsSuccess = false;
                    authModel.Message = "Invalid code!";
                    return authModel;
                }
                else
                {
                    if (smsCode.IsUsed)
                    {
                        authModel.IsSuccess = false;
                        authModel.Message = "Invalid code!";
                        return authModel;
                    }
                    if (smsCode.GenerateDate.AddMinutes(2) < DateTime.Now)
                    {
                        authModel.IsSuccess = false;
                        authModel.Message = "Invalid code!";
                        return authModel;
                    }
                    //TODO implement request count validation
                    smsCode.IsUsed = true;
                    smsCode.RequestCount++;

                    var user = _authenticationServices.FindByPhonenumberAsync(phonenumber).Result;
                    if (user == null)
                    {
                        user = _serviceWrapper.UserRepository.AddAsync(new User { Phonenumber = phonenumber}).Result;
                        var userToken = _authenticationServices.GenerateTokenAsync(user).Result;
                        if(userToken == null)
                        {
                            authModel.IsSuccess = false;
                            authModel.Message = "Invalid code!";
                            return authModel;
                        }
                        authModel.Tokens = _mapper.Map<TokenGenerateModel>(userToken); ;
                        authModel.RefreshTokenExpiration = userToken.RefreshTokenExpiration;
                        await _tokenServices.SaveTokenAsync(userToken);
                        authModel.IsSuccess = true;
                    }
                    else
                    {
                        var userToken = _authenticationServices.GenerateTokenAsync(user).Result;
                        if(userToken == null)
                        {
                            authModel.IsSuccess = false;
                            authModel.Message = "Invalid code!";
                            return authModel;
                        }
                        authModel.Tokens = _mapper.Map<TokenGenerateModel>(userToken);
                        authModel.RefreshTokenExpiration = userToken.RefreshTokenExpiration;
                        await _tokenServices.SaveTokenAsync(userToken);
                        authModel.IsSuccess = true;
                    }
                }

                return authModel;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LogOut(string userPhone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userPhone))
                    throw new ArgumentNullException(nameof(userPhone));

                var user = await _authenticationServices.FindByPhonenumberAsync(userPhone) ?? throw new ArgumentException("User not found", nameof(userPhone));

                await _tokenServices.DeleteRangeTokensAsync(user.Id);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
