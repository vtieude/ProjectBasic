using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Core.DAL.EntityFramework
{
    public class UnitOfWorkFactory<T> : IUnitOfWorkFactory where T : DbContext
    {
        private readonly Func<T> _dbContextFactory;
        private readonly IUserContext _userContext;

        public UnitOfWorkFactory(Func<T> dbContextFactory, IUserContext userContext)
        {
            _dbContextFactory = dbContextFactory;
            _userContext = userContext;
        }

        public IUnitOfWork Create(Action action)
        {
            IUnitOfWork uof = new UnitOfWork<T>(_dbContextFactory.Invoke(), _userContext.CurrentUser.UserName);
            if (action != null)
            {
                uof.Execute(action);
            }

            return uof;
        }

        public async Task<IUnitOfWork> Create(Func<Task> action)
        {
            IUnitOfWork uof = new UnitOfWork<T>(_dbContextFactory.Invoke(), _userContext.CurrentUser.UserName);
            if (action != null)
            {
                await uof.Execute(action);
            }

            return uof;
        }
    }
}
