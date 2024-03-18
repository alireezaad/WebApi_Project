using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaskEntityUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int TaskId { get; set; }
        public TaskEntity TaskEntity { get; set; }

    }
}
