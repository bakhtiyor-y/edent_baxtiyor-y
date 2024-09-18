using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Services.Base
{
    public interface IEntityService<TEntity>
        where TEntity : Entity
    {
        IRepository<TEntity> Repository { get; }
        IMapper Mapper { get; }
        IQueryable<TEntity> Query();
        Task<TEntity> GetByIdAsync(int id);
        TEntity GetById(int id);
        TViewModel GetById<TViewModel>(int id) where TViewModel : IViewModel;
        IEnumerable<TEntity> GetAll();
        IEnumerable<TViewModel> GetAll<TViewModel>() where TViewModel : IViewModel;
        TEntity Create<TViewModel>(TViewModel model) where TViewModel : IViewModel;
        TEntity Create(TEntity entity);
        bool CreateRange(IEnumerable<TEntity> entities);
        TEntity Update<TViewModel>(int id, TViewModel model) where TViewModel : IViewModel;
        bool UpdateRange(IEnumerable<TEntity> entities);
        TEntity Save<TViewModel>(TViewModel model) where TViewModel : IViewModel;
        bool Delete(int id);
        bool Delete(TEntity entity);

    }
}
