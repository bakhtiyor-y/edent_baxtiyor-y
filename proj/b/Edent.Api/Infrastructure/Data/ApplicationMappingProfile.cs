using AutoMapper;
using Edent.Api.Helpers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Infrastructure.Resolvers;
using Edent.Api.ViewModels;
using Edent.Api.ViewModels.Account;
using System;
using System.Linq;

namespace Edent.Api.Infrastructure.Data
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            #region Entity to ViewModel

            CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(f => f.PatientFullName, opt => opt.MapFrom(m => m.Patient != null ? $"{m.Patient.LastName} {m.Patient.FirstName} {m.Patient.Patronymic}" : string.Empty))
                .ForMember(f => f.PatientBirthDate, opt => opt.MapFrom(m => m.Patient != null ? m.Patient.BirthDate : DateTime.Now))
                .ForMember(f => f.PatientPhoneNumber, opt => opt.MapFrom(m => m.Patient != null ? m.Patient.PhoneNumber : string.Empty))
                .ForMember(f => f.DoctorFullName, opt => opt.MapFrom(m => m.Doctor != null ? $"{m.Doctor.LastName} {m.Doctor.FirstName} {m.Doctor.Patronymic}" : string.Empty))
                .ForMember(f => f.EmployeeFullName, opt => opt.MapFrom(m => m.Employee != null ? $"{m.Employee.LastName} {m.Employee.FirstName} {m.Employee.Patronymic}" : string.Empty));

            CreateMap<User, UserViewModel>()
                .ForMember(f => f.ProfileImage, opt => opt.MapFrom<UrlResolver>())
                .AfterMap((entity, viewModel) =>
                {
                    if (!string.IsNullOrWhiteSpace(entity.ProfileImage))
                    {
                        viewModel.ProfileImage = $"{viewModel.ProfileImage}{MediaUrls.GetUserMedia(entity.ProfileImage, entity.Id)}";
                    }
                });

            CreateMap<Employee, UserViewModel>()
               .ForMember(f => f.Id, opt => opt.MapFrom(m => m.User != null ? m.User.Id : 0))
               .ForMember(f => f.EmployeeId, opt => opt.MapFrom(m => m.Id))
               .ForMember(f => f.ProfileImage, opt => opt.MapFrom<UrlResolver>())
               .ForMember(f => f.UserName, opt => opt.MapFrom(m => m.User != null ? m.User.UserName : string.Empty))
               .ForMember(f => f.Email, opt => opt.MapFrom(m => m.User != null ? m.User.Email : string.Empty))
               .ForMember(f => f.PhoneNumber, opt => opt.MapFrom(m => m.User != null ? m.User.PhoneNumber : string.Empty))
               .ForMember(f => f.IsActive, opt => opt.MapFrom(m => m.User != null ? m.User.IsActive : false))
               .AfterMap((entity, viewModel) =>
               {
                   if (entity.User != null && !string.IsNullOrWhiteSpace(entity.User.ProfileImage))
                   {
                       viewModel.ProfileImage = $"{viewModel.ProfileImage}{MediaUrls.GetUserMedia(entity.User.ProfileImage, entity.User.Id)}";
                   }
               });

            CreateMap<Doctor, UserViewModel>()
               .ForMember(f => f.Id, opt => opt.MapFrom(m => m.User != null ? m.User.Id : 0))
               .ForMember(f => f.ProfileImage, opt => opt.MapFrom<UrlResolver>())
               .ForMember(f => f.UserName, opt => opt.MapFrom(m => m.User != null ? m.User.UserName : string.Empty))
               .ForMember(f => f.Email, opt => opt.MapFrom(m => m.User != null ? m.User.Email : string.Empty))
               .ForMember(f => f.PhoneNumber, opt => opt.MapFrom(m => m.User != null ? m.User.PhoneNumber : string.Empty))
               .ForMember(f => f.IsActive, opt => opt.MapFrom(m => m.User != null ? m.User.IsActive : false))
               .AfterMap((entity, viewModel) =>
               {
                   if (entity.User != null && !string.IsNullOrWhiteSpace(entity.User.ProfileImage))
                   {
                       viewModel.ProfileImage = $"{viewModel.ProfileImage}{MediaUrls.GetUserMedia(entity.User.ProfileImage, entity.User.Id)}";
                   }
               });

            CreateMap<Role, RoleViewModel>();
            CreateMap<Country, CountryViewModel>();
            CreateMap<Region, RegionViewModel>();
            CreateMap<City, CityViewModel>();
            CreateMap<Inventory, InventoryViewModel>()
                .ForMember(f => f.Stock, opt => opt.MapFrom(m => m.MeasurementUnit != null ? (m.Stock / m.MeasurementUnit.Multiplicity) : m.Stock))
                .AfterMap((entity, viewModel) =>
                {
                    var lastPrice = entity.InventoryPrices.OrderByDescending(o => o.DateFrom).FirstOrDefault();
                    viewModel.CurrentPrice = lastPrice != null ? lastPrice.Price : 0;
                });

            CreateMap<InventoryPrice, InventoryPriceViewModel>();
            CreateMap<MeasurementUnitType, MeasurementUnitTypeViewModel>();
            CreateMap<MeasurementUnit, MeasurementUnitViewModel>();
            CreateMap<DentalService, DentalServiceViewModel>()
                .ForMember(f => f.CategoryName, opt => opt.MapFrom(m => m.DentalServiceCategory != null ? m.DentalServiceCategory.Name : string.Empty))
                .ForMember(f => f.GroupName, opt => opt.MapFrom(m => m.DentalServiceGroup != null ? m.DentalServiceGroup.Name : string.Empty))
                .AfterMap((entity, viewModel) =>
                {
                    var lastPrice = entity.DentalServicePrices.OrderByDescending(o => o.DateFrom).FirstOrDefault();
                    viewModel.CurrentPrice = lastPrice != null ? lastPrice.Price : 0;
                    foreach (var item in entity.DentalServiceReceptInventorySettings)
                    {
                        viewModel.ReceptInventorySettings.Add(item.ReceptInventorySettingId);
                    }
                });

            CreateMap<DentalServiceCategory, DentalServiceCategoryViewModel>()
                .ForMember(f => f.DentalServices, opt => opt.Ignore());
            CreateMap<DentalServiceGroup, DentalServiceGroupViewModel>();
            CreateMap<DentalServicePrice, DentalServicePriceViewModel>();
            CreateMap<Diagnose, DiagnoseViewModel>();
            CreateMap<Specialization, SpecializationViewModel>();
            CreateMap<Profession, ProfessionViewModel>();
            CreateMap<Term, TermViewModel>();
            CreateMap<ReceptInventory, ReceptInventoryViewModel>();

            CreateMap<ReceptInventorySetting, ReceptInventorySettingViewModel>();
            CreateMap<ReceptInventorySettingItem, ReceptInventorySettingItemViewModel>()
                .ForMember(f => f.SelectedInventory, opt => opt.MapFrom(m => m.Inventory));

            CreateMap<Organization, OrganizationViewModel>()
                .ForMember(f => f.LogoImage, opt => opt.MapFrom<UrlResolver>())
                .AfterMap((entity, viewModel) =>
                {
                    if (!string.IsNullOrWhiteSpace(entity.LogoImage))
                    {
                        viewModel.LogoImage = $"{viewModel.LogoImage}{MediaUrls.GetSharedImage(entity.LogoImage)}";
                    }
                });

            CreateMap<Address, AddressViewModel>();
            CreateMap<Tooth, ToothViewModel>();


            CreateMap<Doctor, DoctorViewModel>()
                .ForMember(f => f.PhoneNumber, opt => opt.MapFrom(m => m.User != null ? m.User.PhoneNumber : string.Empty))
                .ForMember(f => f.Email, opt => opt.MapFrom(m => m.User != null ? m.User.Email : string.Empty))
                .ForMember(f => f.IsActive, opt => opt.MapFrom(m => m.User != null ? m.User.IsActive : false))
                .ForMember(f => f.ProfileImage, opt => opt.MapFrom<UrlResolver>())
                .ForMember(f => f.Specialization, opt => opt.MapFrom(m => m.Specialization != null ? m.Specialization.Name : string.Empty))
                .AfterMap((entity, viewModel) =>
                {
                    if (entity.User != null && !string.IsNullOrWhiteSpace(entity.User.ProfileImage))
                    {
                        viewModel.ProfileImage = $"{viewModel.ProfileImage}{MediaUrls.GetUserMedia(entity.User.ProfileImage, entity.UserId)}";
                    }
                    var doctorInTerm = entity.DoctorInTerms.FirstOrDefault();
                    if (doctorInTerm != null && doctorInTerm.Term != null)
                        viewModel.Term = doctorInTerm.Term.Name;
                });

            CreateMap<Doctor, DoctorManageViewModel>()
                .ForMember(f => f.PhoneNumber, opt => opt.MapFrom(m => m.User != null ? m.User.PhoneNumber : string.Empty))
                .ForMember(f => f.Email, opt => opt.MapFrom(m => m.User != null ? m.User.Email : string.Empty))
                .ForMember(f => f.IsActive, opt => opt.MapFrom(m => m.User != null ? m.User.IsActive : false))
                .AfterMap((entity, viewModel) =>
                {
                    var doctorInTerm = entity.DoctorInTerms.FirstOrDefault();
                    if (doctorInTerm != null && doctorInTerm.Term != null)
                    {
                        viewModel.TermId = doctorInTerm.TermId;
                        viewModel.TermValue = doctorInTerm.TermValue;
                    }

                    var doctorAddress = entity.DoctorAddresses.FirstOrDefault();
                    if (doctorAddress != null && doctorAddress.Address != null)
                    {
                        viewModel.AddressLine1 = doctorAddress.Address.AddressLine1;
                        viewModel.AddressLine2 = doctorAddress.Address.AddressLine2;
                        viewModel.CityId = doctorAddress.Address.CityId;
                        if (doctorAddress.Address.City != null)
                        {
                            viewModel.RegionId = doctorAddress.Address.City.RegionId;
                            if (doctorAddress.Address.City.Region != null)
                            {
                                viewModel.CountryId = doctorAddress.Address.City.Region.CountryId;
                            }
                        }
                    }
                    if (entity.DoctorDentalChairs != null)
                    {
                        viewModel.DentalChairs = entity.DoctorDentalChairs.Select(s => s.DentalChairId).ToHashSet();
                    }
                });

            CreateMap<Partner, PartnerViewModel>();

            CreateMap<PatientTooth, PatientToothViewModel>();

            CreateMap<Patient, PatientViewModel>()
                .ForMember(f => f.FullName, opt => opt.MapFrom(m => $"{m.LastName} {m.FirstName} {m.Patronymic}"))
                .AfterMap((entity, viewModel) =>
                {
                    var patientAddress = entity.PatientAddresses.FirstOrDefault();
                    if (patientAddress != null && patientAddress.Address != null)
                    {
                        viewModel.Address = $"{patientAddress.Address.City?.Region?.Name}, {patientAddress.Address.City?.Name}, {patientAddress.Address.AddressLine1}, {patientAddress.Address.AddressLine2}";
                    }
                });

            CreateMap<Patient, PatientManageViewModel>()
                .AfterMap((entity, viewModel) =>
                {
                    var patientAddress = entity.PatientAddresses.FirstOrDefault();
                    if (patientAddress != null && patientAddress.Address != null)
                    {
                        viewModel.AddressLine1 = patientAddress.Address.AddressLine1;
                        viewModel.AddressLine2 = patientAddress.Address.AddressLine2;
                        viewModel.CityId = patientAddress.Address.CityId;
                        if (patientAddress.Address.City != null)
                        {
                            viewModel.RegionId = patientAddress.Address.City.RegionId;
                            if (patientAddress.Address.City.Region != null)
                            {
                                viewModel.CountryId = patientAddress.Address.City.Region.CountryId;
                            }
                        }
                    }
                });

            CreateMap<Schedule, ScheduleViewModel>();
            CreateMap<ScheduleSetting, ScheduleSettingViewModel>()
                .ForMember(f => f.FromTime, opt => opt.MapFrom(m => m.FromMinute.ToString(@"hh\:mm")))
                .ForMember(f => f.ToTime, opt => opt.MapFrom(m => m.ToMinute.ToString(@"hh\:mm")))
                .ForMember(f => f.SettingDayOfWeeks, opt => opt.MapFrom(m => m.SettingDayOfWeeks.Select(s => s.DayOfWeek)));
            CreateMap<SettingDayOfWeek, SettingDayOfWeekViewModel>();

            CreateMap<Recept, ReceptViewModel>();
            CreateMap<Technic, TechnicViewModel>();
            CreateMap<Treatment, TreatmentViewModel>();
            CreateMap<TreatmentDentalService, TreatmentDentalServiceViewModel>();


            CreateMap<Invoice, InvoiceViewModel>()
                .ForMember(f => f.PatientFullName, opt => opt.MapFrom(m => (m.Recept != null && m.Recept.Patient != null) ? $"{m.Recept.Patient.LastName} {m.Recept.Patient.FirstName} {m.Recept.Patient.Patronymic}" : string.Empty))
                .ForMember(f => f.ReceptDate, opt => opt.MapFrom(m => m.Recept != null ? m.Recept.CreatedDate : m.CreatedDate))
                .ForMember(f => f.PatientBalance, opt => opt.MapFrom(m => (m.Recept != null && m.Recept.Patient != null) ? m.Recept.Patient.Balance : 0))
                .ForMember(f => f.PhoneNumber, opt => opt.MapFrom(m => (m.Recept != null && m.Recept.Patient != null) ? $"{m.Recept.Patient.PhoneNumber}" : string.Empty))
                .AfterMap((entity, viewModel) =>
                {
                    if (entity.Recept == null)
                    {
                        return;
                    }

                    if (entity.Recept.Doctor != null)
                    {
                        viewModel.DoctorFullName = $"{entity.Recept.Doctor.LastName} {entity.Recept.Doctor.FirstName} {entity.Recept.Doctor.Patronymic}";
                    }
                    else if (entity.Recept.Employee != null)
                    {
                        viewModel.DoctorFullName = $"{entity.Recept.Employee.LastName} {entity.Recept.Employee.FirstName} {entity.Recept.Employee.Patronymic}";
                    }
                });

            CreateMap<Payment, PaymentViewModel>();
            CreateMap<InventoryOutcome, InventoryOutcomeViewModel>()
                .AfterMap((entity, viewModel) =>
                {

                    if (entity.User != null && entity.User.Doctor != null)
                    {
                        var firstName = entity.User.Doctor.FirstName[0].ToString();
                        var patronymic = !string.IsNullOrWhiteSpace(entity.User.Doctor.Patronymic) && entity.User.Doctor.Patronymic.Length > 1 ? entity.User.Doctor.Patronymic[0].ToString() : "";
                        viewModel.Who = $"{entity.User.Doctor.LastName} {firstName} {patronymic}";
                    }
                    else if (entity.User != null && entity.User.Employee != null)
                    {
                        var firstName = entity.User.Employee.FirstName[0].ToString();
                        var patronymic = !string.IsNullOrWhiteSpace(entity.User.Employee.Patronymic) && entity.User.Employee.Patronymic.Length > 1 ? entity.User.Employee.Patronymic[0].ToString() : "";
                        viewModel.Who = $"{entity.User.Employee.LastName} {firstName} {patronymic}";
                    }

                    if (entity.Recipient != null && entity.Recipient.Doctor != null)
                    {
                        var firstName = entity.Recipient.Doctor.FirstName[0].ToString();
                        var patronymic = !string.IsNullOrWhiteSpace(entity.Recipient.Doctor.Patronymic) && entity.Recipient.Doctor.Patronymic.Length > 1 ? entity.Recipient.Doctor.Patronymic[0].ToString() : "";
                        viewModel.Whom = $"{entity.Recipient.Doctor.LastName} {firstName} {patronymic}";
                    }
                    else if (entity.Recipient != null && entity.Recipient.Employee != null)
                    {
                        var firstName = entity.Recipient.Employee.FirstName[0].ToString();
                        var patronymic = !string.IsNullOrWhiteSpace(entity.Recipient.Employee.Patronymic) && entity.Recipient.Employee.Patronymic.Length > 1 ? entity.Recipient.Employee.Patronymic[0].ToString() : "";
                        viewModel.Whom = $"{entity.Recipient.Employee.LastName} {firstName} {patronymic}";
                    }

                    foreach (var item in entity.InventoryItems)
                    {
                        if (item.Inventory == null)
                            continue;

                        var price = item.Inventory.InventoryPrices
                        .OrderByDescending(o => o.DateFrom)
                        .FirstOrDefault(f => entity.CreatedDate >= f.DateFrom);

                        if (price != null)
                        {
                            viewModel.TotalCost += price.Price * item.Quantity;
                        }
                    }
                });

            CreateMap<OutcomeInventoryItem, OutcomeInventoryItemViewModel>();

            CreateMap<InventoryIncome, InventoryIncomeViewModel>()
                .AfterMap((entity, viewModel) =>
                {
                    if (entity.User != null && entity.User.Doctor != null)
                    {
                        var firstName = entity.User.Doctor.FirstName[0].ToString();
                        var patronymic = !string.IsNullOrWhiteSpace(entity.User.Doctor.Patronymic) && entity.User.Doctor.Patronymic.Length > 1 ? entity.User.Doctor.Patronymic[0].ToString() : "";
                        viewModel.Who = $"{entity.User.Doctor.LastName} {firstName} {patronymic}";
                    }
                    else if (entity.User != null && entity.User.Employee != null)
                    {
                        var firstName = entity.User.Employee.FirstName[0].ToString();
                        var patronymic = !string.IsNullOrWhiteSpace(entity.User.Employee.Patronymic) && entity.User.Employee.Patronymic.Length > 1 ? entity.User.Employee.Patronymic[0].ToString() : "";
                        viewModel.Who = $"{entity.User.Employee.LastName} {firstName} {patronymic}";
                    }

                    foreach (var item in entity.InventoryItems)
                    {
                        if (item.Inventory == null)
                            continue;

                        var price = item.Inventory.InventoryPrices
                        .OrderByDescending(o => o.DateFrom)
                        .FirstOrDefault(f => entity.CreatedDate >= f.DateFrom);

                        if (price != null)
                        {
                            viewModel.TotalCost += price.Price * item.Quantity;
                        }
                    }
                });

            CreateMap<IncomeInventoryItem, IncomeInventoryItemViewModel>();
            CreateMap<DentalChair, DentalChairViewModel>();
            CreateMap<ReceptDentalService, ReceptDentalServiceViewModel>();

            #endregion

            #region ViewModel to Entity

            CreateMap<CountryViewModel, Country>();
            CreateMap<RegionViewModel, Region>();
            CreateMap<CityViewModel, City>();
            CreateMap<InventoryViewModel, Inventory>();
            CreateMap<InventoryPriceViewModel, InventoryPrice>();
            CreateMap<MeasurementUnitTypeViewModel, MeasurementUnitType>();
            CreateMap<MeasurementUnitViewModel, MeasurementUnit>();
            CreateMap<DentalServiceViewModel, DentalService>();
            CreateMap<DentalServiceCategoryViewModel, DentalServiceCategory>();
            CreateMap<DentalServiceGroupViewModel, DentalServiceGroup>();
            CreateMap<DentalServicePriceViewModel, DentalServicePrice>();
            CreateMap<DiagnoseViewModel, Diagnose>();
            CreateMap<SpecializationViewModel, Specialization>();
            CreateMap<ProfessionViewModel, Profession>();
            CreateMap<TermViewModel, Term>();
            CreateMap<ReceptInventorySettingViewModel, ReceptInventorySetting>();
            CreateMap<ReceptInventorySettingItemViewModel, ReceptInventorySettingItem>();

            CreateMap<OrganizationViewModel, Organization>()
                .ForMember(f => f.LogoImage, opt => opt.Ignore());

            CreateMap<AddressViewModel, Address>();
            CreateMap<ToothViewModel, Tooth>();
            CreateMap<PartnerViewModel, Partner>();

            CreateMap<ScheduleViewModel, Schedule>()
                .ForMember(f => f.ScheduleSettings, opt => opt.Ignore());

            CreateMap<ScheduleSettingViewModel, ScheduleSetting>()
                    .ForMember(f => f.FromMinute, opt => opt.MapFrom(m => TimeSpan.Parse(m.FromTime)))
                    .ForMember(f => f.ToMinute, opt => opt.MapFrom(m => TimeSpan.Parse(m.ToTime)))
                    .ForMember(f => f.SettingDayOfWeeks, opt => opt.Ignore());

            CreateMap<SettingDayOfWeekViewModel, SettingDayOfWeek>();

            CreateMap<ReceptViewModel, Recept>();
            CreateMap<TechnicViewModel, Technic>();
            CreateMap<TreatmentViewModel, Treatment>()
                .ForMember(f => f.Diagnose, opt => opt.Ignore())
                .ForMember(f => f.PatientTooth, opt => opt.Ignore());

            CreateMap<ReceptInventoryViewModel, ReceptInventory>()
                    .ForMember(f => f.Inventory, opt => opt.Ignore())
                    .ForMember(f => f.MeasurementUnit, opt => opt.Ignore())
                    .ForMember(f => f.Recept, opt => opt.Ignore());

            CreateMap<TreatmentDentalServiceViewModel, TreatmentDentalService>();

            CreateMap<InventoryOutcomeViewModel, InventoryOutcome>()
                .ForMember(f => f.Recept, opt => opt.Ignore());

            CreateMap<OutcomeInventoryItemViewModel, OutcomeInventoryItem>()
                .ForMember(f => f.Inventory, opt => opt.Ignore())
                .ForMember(f => f.MeasurementUnit, opt => opt.Ignore());

            CreateMap<InventoryIncomeViewModel, InventoryIncome>();

            CreateMap<IncomeInventoryItemViewModel, IncomeInventoryItem>()
                .ForMember(f => f.Inventory, opt => opt.Ignore())
                .ForMember(f => f.MeasurementUnit, opt => opt.Ignore());

            CreateMap<DentalChairViewModel, DentalChair>();
            CreateMap<ReceptDentalServiceViewModel, ReceptDentalService>();

            #endregion

        }
    }
}
