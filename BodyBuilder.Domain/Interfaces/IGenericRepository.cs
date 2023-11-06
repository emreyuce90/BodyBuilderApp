using BodyBuilder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Interfaces {
    public interface IGenericRepository<T> where T:BaseEntity,new(){
        Task<T?> CreateAsync(T entity);
        T? UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid Id);
        bool DeleteAsync(T entity); 

        public DbSet<T> Table { get; }
        IQueryable<T> GetAllAsync(bool isTracking=true);
        IQueryable<T> GetAllAsync(Expression<Func<T,bool>> expression,bool isTracking=true);
        Task<T?> GetSingle(Expression<Func<T,bool>> expression,bool isTracking=true);
        Task<T?> GetById(Guid Id);
        Task<int> CountAsync(Expression<Func<T,bool>> expression);
        Task<int> SaveAsync();
    }
}
