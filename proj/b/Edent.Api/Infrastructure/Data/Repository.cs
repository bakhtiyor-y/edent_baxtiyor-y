using Edent.Api.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {

        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ApplicationDbContext Context
        {
            get
            {
                return _context;
            }
        }        

        public DbSet<TEntity> Query()
        {
            return _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsEnumerable();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().FirstOrDefault(a => a.Id == id);
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entity)
        {
            _context.Set<TEntity>().AddRange(entity);
        }

        public void Edit(TEntity entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<TEntity>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public void EditRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Edit(item);
            }
        }

        public void ChangeEntryState(TEntity entity, EntityState state)
        {
            _context.Entry(entity).State = state;
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(params TEntity[] entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
