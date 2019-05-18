using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Core.DAL
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(Action action = null);
        Task<IUnitOfWork> Create(Func<Task> action);
    }
}
