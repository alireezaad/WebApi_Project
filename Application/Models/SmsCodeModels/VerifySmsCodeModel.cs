using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.SmsCodeModels
{
    public class VerifySmsCodeModel
    {
        public string Phonenumber { get; set; }
        public string Code { get; set; }
    }
}
