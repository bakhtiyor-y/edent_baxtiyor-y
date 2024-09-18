using Edent.Api.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edent.Api.Infrastructure.Data
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        public ApplicationDbContext Context { get; }

        DbSet<TEntity> Query();

        public IEnumerable<TEntity> GetAll();
        public TEntity GetById(int id);

        public void Add(TEntity entity);

        public void AddRange(IEnumerable<TEntity> entity);

        public void Edit(TEntity entity);

        public void EditRange(IEnumerable<TEntity> entities);

        public void ChangeEntryState(TEntity entity, EntityState state);

        public void Remove(TEntity entity);

        public void RemoveRange(params TEntity[] entities);

        public bool SaveChanges();

        public Task<int> SaveChangesAsync();
    }
}
