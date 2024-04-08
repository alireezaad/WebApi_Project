using Kavenegar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SmsService
    {
        private readonly KavenegarApi _kaveNegar;
        public SmsService() 
        {
            _kaveNegar = new KavenegarApi("");
        }

        public void SendAsync(string receptor, string code)
        {
            _kaveNegar.VerifyLookup(receptor, code, "SendApiVerificationCode");
        }
    }
}
