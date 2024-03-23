using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.TokenModels
{
    public class AuthenticateResultModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TokenGenerateModel Tokens { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
