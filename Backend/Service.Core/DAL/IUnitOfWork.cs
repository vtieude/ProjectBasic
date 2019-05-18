using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Core.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Execute(Action run);
        Task Execute(Func<Task> run);
        void Rollback();
    }
}
