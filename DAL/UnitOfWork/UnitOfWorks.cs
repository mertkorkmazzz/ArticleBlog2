using BlogApp.DAL.Context;
using BlogApp.DAL.Repositories.Abstractions;
using BlogApp.DAL.Repositories.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DAL.UnitOfWork
{
    public class UnitOfWorks : IUnitOfWork
    {


        private readonly AppDbContext _dbContext;

        public UnitOfWorks(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }



        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        IRepository<T> IUnitOfWork.GetRepository<T>()
        {
            return new Repository<T>(_dbContext);
        }
    }
}
