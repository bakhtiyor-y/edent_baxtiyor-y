﻿using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.ViewModels;

namespace Edent.Api.Services.EntityServices.Interfaces
{
    public interface IDoctorService : IEntityService<Doctor>
    {
        DoctorViewModel GetByIncluding(int id);
        Doctor GetByUserId(int userId);
    }
}
