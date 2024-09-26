using BlogApp.Core.Entities;
using BlogApp.DAL.Context;
using BlogApp.DAL.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DAL.Repositories.Concretes
{
    public class Repository<T>:IRepository<T> where T : class , IEntityBase, new()
    {
        //Kod, genellikle veri tabanı işlemlerini soyutlamak ve daha temiz, yeniden kullanılabilir bir kod yapısı sağlamak için kullanılır. 
        //Entity Framework Core kullanılarak, veritabanı işlemleri (ekleme, silme, güncelleme, listeleme, sayma vb.) basit ve tutarlı bir şekilde yapılır.



        // alttaki ctor... olmasaydı repository sınıfı veritabanı ile etkileşimde bulunamazdı.
        private readonly AppDbContext _dbContext;

        public Repository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private DbSet<T> Table { get=> _dbContext.Set<T>() ; }




        //Amacı: Tüm kayıtları getirir, isteğe bağlı olarak bir koşul (predicate) ve ilişkili verileri de içerecek şekilde (includeProperties) döner.
        public async Task<List<T>> GetAllAsync(Expression<Func<T , bool>> predicate = null ,params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Table;
            if(predicate != null )
                query = query.Where(predicate);
            
            if(includeProperties.Any())
                foreach(var item in includeProperties)
                    query = query.Include(item);

            return await query.ToListAsync();
        }

        //Amacı: Yeni bir kayıt ekler.
        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        //Amacı: Belirli bir koşula göre tek bir kayıt getirir, isteğe bağlı olarak ilişkili verileri de içerebilir.
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate , params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Table;
            query = query.Where(predicate);

            if(includeProperties.Any())
                foreach(var item in includeProperties)
                    query = query.Include(item);

            return await query.FirstOrDefaultAsync();
        }

        //Amacı: Belirli bir GUID'e sahip tek bir kaydı getirir.
        public async Task<T> GetByGuidAsync(Guid id)
        {
            return await Table.FindAsync(id);
        }

        //Amacı: Mevcut bir kaydı güncelle
        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => Table.Update(entity));
            return entity;
        }

        //Amacı: Mevcut bir kaydı güncelle
        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => Table.Remove(entity));
        }

        //Amacı: Belirli bir koşula uyan herhangi bir kaydın olup olmadığını kontrol eder.
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.AnyAsync(predicate);
        }

        //Amacı: Belirli bir koşula uyan kayıtların sayısını döndürür.
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate is not null)
                return await Table.CountAsync(predicate);
            return await Table.CountAsync();
        }




        
    }
}
