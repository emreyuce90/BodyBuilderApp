using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Infrastructure.Persistence.Repositories {
    public class EfGenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new() {
        //Context injection
        private readonly BodyBuilderContext _context;

        public EfGenericRepository(BodyBuilderContext context) {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression) {
            var count = await Table.CountAsync(expression);
            return count;
        }

        public async Task<T?> CreateAsync(T entity) {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            if (entityEntry.State == EntityState.Added) {
                return entity;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(Guid Id) {
            var deleted = await Table.FirstOrDefaultAsync(x => x.Id == Id);
            if (deleted != null) {
                Table.Remove(deleted);
                return true;

            }
            return false;
        }

        public bool DeleteAsync(T entity) {
           EntityEntry<T> entityEntry = Table.Remove(entity);
            if(entityEntry.State == EntityState.Deleted) {
                return true;
            }
            return false;

        }

        public  IQueryable<T> GetAllAsync(bool isTracking = true) {
            var query = Table.AsQueryable();
            if (!isTracking) {
                query = query.AsNoTracking();
            }
            return query.AsQueryable();
        }

        public IQueryable<T> GetAllAsync(Expression<Func<T, bool>> expression, bool isTracking = true) {
            var query = Table.AsQueryable();
            if (!isTracking) {
                query = query.AsNoTracking();
            }
            return query.Where(expression);
        }

        public async Task<T?> GetById(Guid Id)=> await Table.FirstOrDefaultAsync(t => t.Id == Id);

        public async Task<T?> GetSingle(Expression<Func<T, bool>> expression, bool isTracking = true) {
            var query = Table.AsQueryable();
            if (!isTracking) {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<int> SaveAsync() {
            var save = await _context.SaveChangesAsync();
            return save;
        }

        public T? UpdateAsync(T entity) {
           EntityEntry<T> entityEntry = Table.Update(entity);
            if (entityEntry.State == EntityState.Modified) {
                return entity;

            }
            return null;
        }
    }
}
