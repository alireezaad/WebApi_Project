using Application.UseCases.Managers;
using Application.UseCases.TaskEntityUseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class TaskEntityUseCaseManager : ITaskEntityUseCaseManager
    {
        private CreateTaskEntityUC _createTaskEntity;
        private GetAllTaskEntityUC _getAllTaskEntity;

        public CreateTaskEntityUC CreateTaskEntityUC => _createTaskEntity;
        public GetAllTaskEntityUC GetAllTaskEntityUC => _getAllTaskEntity;

        public TaskEntityUseCaseManager(CreateTaskEntityUC createTaskEntityUC, GetAllTaskEntityUC getAllTaskEntityUC)
        {
            _createTaskEntity = createTaskEntityUC;
            _getAllTaskEntity = getAllTaskEntityUC;
        }
    }
}
