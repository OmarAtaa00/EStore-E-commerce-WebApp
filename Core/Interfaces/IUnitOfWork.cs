using System;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable //> look for a dispose method and when we finish our transaction, dispose  context
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        //after doing staff, call this method to save to the database then
        // return the number of changes 
        Task<int> Complete();
        
    }
}
