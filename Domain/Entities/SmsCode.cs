using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SmsCode
    {
        public Guid Id  { get; set; }
        public string Phonenumber { get; set; }
        public string Code { get; set; }
        public bool IsUsed { get; set; }
        public int RequestCount { get; set; }
        public DateTime GenerateDate { get; set; }

    }
}
