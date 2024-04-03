using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public User()
        {
            this.Tasks = new List<TaskEntity>();
        }
        public int Id { get; set; }
        public string Phonenumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; }
        public ICollection<UserToken> UserTokens { get; set; }
        //public ICollection<TaskEntityUser> TaskEntityUsers { get; set; }

        public void AddTaskEntity(TaskEntity taskEntity)
        {
            if (!Tasks.Contains(taskEntity))
            {
                this.Tasks.Add(taskEntity);
            }
        }

        public void RemoveTaskEntity(TaskEntity taskEntity)
        {
            if (Tasks.Contains(taskEntity))
            {
                this.Tasks.Remove(taskEntity);
            }
        }
    }
}
