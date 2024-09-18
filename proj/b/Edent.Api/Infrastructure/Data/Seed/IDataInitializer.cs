using System;
using System.Threading.Tasks;

namespace Edent.Api.Infrastructure.Data.Seed
{
    public interface IDataInitializer
    {
        Task InitializeAsync(IServiceProvider serviceProvider);
    }
}
