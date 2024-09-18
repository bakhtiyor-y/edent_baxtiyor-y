using AutoMapper;
using Edent.Api.Infrastructure.Data;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Services.Base;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Services.EntityServices.Implementations
{
    public class PatientService : EntityService<Patient>, IPatientService
    {
        private readonly IAddressService _addressService;
        private readonly IPatientAddressService _patientAddressService;

        public PatientService(IRepository<Patient> repository, IMapper mapper, IAddressService addressService,
            IPatientAddressService patientAddressService)
            : base(repository, mapper)
        {
            _addressService = addressService;
            _patientAddressService = patientAddressService;
        }

        public PatientViewModel CreatePatient(PatientManageViewModel viewModel)
        {
            Patient patient = new Patient
            {
                Email = viewModel.Email ?? string.Empty,
                PhoneNumber = viewModel.PhoneNumber ?? string.Empty,
                BirthDate = viewModel.BirthDate,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Patronymic = viewModel.Patronymic,
                PatientAgeType = viewModel.PatientAgeType,
                Gender = viewModel.Gender,
                PatientAddresses = new List<PatientAddress>
                    {
                        new PatientAddress
                        {
                            Address = new Address
                            {
                               AddressLine1 = viewModel.AddressLine1,
                               AddressLine2 = viewModel.AddressLine2,
                               CityId = viewModel.CityId
                            }
                        }
                    }
            };

            var createdPatient = Create(patient);
            return GetByIncluding(createdPatient.Id);
        }

        public PatientViewModel GetByIncluding(int id)
        {
            var entity = Query()
                .Include("PatientAddresses.Address.City.Region")
                .FirstOrDefault(f => f.Id == id);

            return Mapper.Map<PatientViewModel>(entity);
        }

        public async Task<PatientViewModel> UpdatePatient(PatientManageViewModel viewModel)
        {
            var patient = GetById(viewModel.Id);
            patient.BirthDate = viewModel.BirthDate;
            patient.FirstName = viewModel.FirstName;
            patient.LastName = viewModel.LastName;
            patient.Patronymic = viewModel.Patronymic;
            patient.PhoneNumber = viewModel.PhoneNumber ?? string.Empty;
            patient.Email = viewModel.Email ?? string.Empty;
            patient.PatientAgeType = viewModel.PatientAgeType;
            patient.Gender = viewModel.Gender;
            Repository.Edit(patient);
            await Repository.SaveChangesAsync();

            var patientAddress = _patientAddressService
                .Query()
                .AsNoTracking()
                .FirstOrDefault(f => f.PatientId == patient.Id) ?? new PatientAddress();

            if (patientAddress.Id == 0)
            {
                patientAddress.Address = new Address { CityId = viewModel.CityId, AddressLine1 = viewModel.AddressLine1, AddressLine2 = viewModel.AddressLine2 };
                _patientAddressService.Repository.Add(patientAddress);
                await _patientAddressService.Repository.SaveChangesAsync();
            }
            else
            {
                var address = _addressService.Query().AsNoTracking().FirstOrDefault(f => f.Id == patientAddress.AddressId);
                address.CityId = viewModel.CityId;
                address.AddressLine1 = viewModel.AddressLine1;
                address.AddressLine2 = viewModel.AddressLine2;
                _addressService.Repository.Edit(address);
                await _addressService.Repository.SaveChangesAsync();
            }

            return GetByIncluding(patient.Id);
        }
    }
}
