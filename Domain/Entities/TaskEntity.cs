using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaskEntity
    {
        public TaskEntity()
        {
            this.Users = new List<User>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
        public bool IsRemoved { get; set; }

        public void AddUser(User user)
        {
            if (!Users.Contains(user))
            {
                this.Users.Add(user);
            }
        }
        public void RemoveUser(User user)
        {
            if (Users.Contains(user))
            {
                Users.Remove(user);
            }
        }
    }
}