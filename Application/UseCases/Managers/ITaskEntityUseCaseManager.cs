using Application.UseCases.TaskEntityUseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Managers
{
    public interface ITaskEntityUseCaseManager
    {
        CreateTaskEntityUC CreateTaskEntityUC {  get; }
        GetAllTaskEntityUC GetAllTaskEntityUC { get; }

    }
}
