using Application.Models.LinkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.UserModels
{
    public class UserGetModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public IEnumerable<int> Tasks { get; set; }

        // For HATEOAS
        public IEnumerable<Link> Links { get; set; }
    }
}
