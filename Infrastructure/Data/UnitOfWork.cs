using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork // unit of work will own StoreContext
    {
        private readonly StoreContext _context;


        // unit of work will crate a new instance of store context and any repo we will use will be stored in this variable 
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;

        }
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //check if any repo in HashTable 
            if (_repositories == null) _repositories = new Hashtable();

            //get the type of the entity 
            var type = typeof(TEntity).Name;

            //check if the Hashtable has entry for this specific name
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                // if we don't have a repository already 
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance); //adding new entry to our Hashtable 
            }
            return (IGenericRepository<TEntity>)_repositories[type]; //brackets cuz we want  to query the entry in the table 

        }
    }
}
