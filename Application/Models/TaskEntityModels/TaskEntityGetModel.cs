using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.TaskEntityModels
{
    public class TaskEntityGetModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> Users { get; set; } = new List<int>();
    }
}
