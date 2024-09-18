using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Services.Base
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : Entity
    {

        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;


        public EntityService(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IRepository<TEntity> Repository => _repository ?? throw new ArgumentNullException();

        public IMapper Mapper => _mapper ?? throw new ArgumentNullException();

        public virtual IQueryable<TEntity> Query()
        {
            return _repository.Query();
        }

        public virtual Task<TEntity> GetByIdAsync(int id)
        {
            return Query().FirstOrDefaultAsync(a => a.Id == id);
        }

        public virtual TEntity GetById(int id)
        {
            return Query().FirstOrDefault(a => a.Id == id);
        }

        public virtual TViewModel GetById<TViewModel>(int id) where TViewModel : IViewModel
        {
            TEntity entity = Query().FirstOrDefault(a => a.Id == id);
            return _mapper.Map<TViewModel>(entity);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Query();
        }

        public virtual IEnumerable<TViewModel> GetAll<TViewModel>() where TViewModel : IViewModel
        {
            var query = Query();
            return _mapper.Map<IEnumerable<TViewModel>>(query);
        }

        public virtual TEntity Create<TViewModel>(TViewModel model) where TViewModel : IViewModel
        {
            TEntity entity = _mapper.Map<TEntity>(model);
            _repository.Add(entity);
            _repository.SaveChanges();

            return entity;
        }

        public virtual TEntity Create(TEntity entity)
        {
            _repository.Add(entity);
            _repository.SaveChanges();
            return entity;
        }

        public virtual bool CreateRange(IEnumerable<TEntity> entities)
        {
            _repository.AddRange(entities);
            return _repository.SaveChanges();
        }

        public virtual TEntity Update<TViewModel>(int id, TViewModel model) where TViewModel : IViewModel
        {
            TEntity entity = _repository.GetById(id);
            entity = _mapper.Map(model, entity);
            _repository.Edit(entity);
            _repository.SaveChanges();
            return entity;
        }

        public virtual bool UpdateRange(IEnumerable<TEntity> entities)
        {
            _repository.EditRange(entities);
            return _repository.SaveChanges();
        }

        public virtual TEntity Save<TViewModel>(TViewModel model) where TViewModel : IViewModel
        {
            if (model.Id > 0)
                return Update(model.Id, model);
            else
                return Create(model);
        }

        public virtual bool Delete(int id)
        {
            TEntity entity = _repository.GetById(id);
            if (entity == null)
                return false;

            _repository.Remove(entity);
            return _repository.SaveChanges();
        }

        public bool Delete(TEntity entity)
        {
            if (entity == null)
                return false;

            _repository.Remove(entity);
            return _repository.SaveChanges();
        }
    }
}
