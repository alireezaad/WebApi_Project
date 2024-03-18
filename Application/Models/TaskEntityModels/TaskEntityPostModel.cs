using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.TaskEntityModels
{
    public class TaskEntityPostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}
