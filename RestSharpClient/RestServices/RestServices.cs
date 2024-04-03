using RestSharp;
using RestSharp.Authenticators;
using RestSharpClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpClient.RestServices
{
    public class RestServices
    {
        private readonly RestClient _restClient;
        public RestServices()
        {
            _restClient = new RestClient("https://localhost:7064");
        }

        public async Task<bool> GetPhonenumber(string phonenumber)
        {
            var request = new RestRequest("api/Account/AuthenticateWithPhonenumber", Method.Get);
            request.AddParameter("phonenumber" ,phonenumber);
            var result = await _restClient.GetAsync(request);
            return result.IsSuccessful;
        }

        public async Task<string> GetSmsCode(string phonenumber, string code)
        {
            var request = new RestRequest("api/Account/VerifyPhonenumber", Method.Post);
            request.AddJsonBody(new { Phonenumber = phonenumber, Code = code });
            var result = await _restClient.PostAsync<TokenGenerateModel>(request);
            return result.Token;
        }

        public async Task<List<UserGetModel>> GetAllUsers(string accesstoken)
        {
            _restClient.Authenticator = new JwtAuthenticator(accesstoken);
            var request = new RestRequest("api/Users/Get", Method.Get);
            var result = await _restClient.GetAsync<List<UserGetModel>>(request);
            return result;

        }
    }
}
