﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpClient.Models
{
    public class TokenGenerateModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
