using AutoMapper;
using Edent.Api.Helpers;
using Edent.Api.Infrastructure.Entities;
using Edent.Api.Models.Seed;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Infrastructure.Data.Seed
{
    public class DataInitializer : IDataInitializer
    {
        private ApplicationDbContext _context;
        private ILogger<DataInitializer> _logger;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private ICityService _cityService;
        private IOrganizationService _organizationService;
        private ISpecializationService _specializationService;
        private ITermService _termService;
        private IMeasurementUnitTypeService _measurementUnitService;
        private IMapper _mapper;

        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {

            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _logger = serviceProvider.GetRequiredService<ILogger<DataInitializer>>();

            _roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            _cityService = serviceProvider.GetRequiredService<ICityService>();
            _organizationService = serviceProvider.GetRequiredService<IOrganizationService>();
            _specializationService = serviceProvider.GetRequiredService<ISpecializationService>();
            _termService = serviceProvider.GetRequiredService<ITermService>();
            _measurementUnitService = serviceProvider.GetRequiredService<IMeasurementUnitTypeService>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            if (_context == null)
                return;

            await SeedRoles();
            await SeedUsers();
            await SeedArea();
            await SeedProfessions();
            await SeedOrganizations();
            await SeedTerms();
            await SeedDoctors();
            await SeedEmployees();
            await SeedMeasurementUnitTypes();
            await SeedInventories();
            await SeedDentalServiceCategories();
            await SeedDentalServiceGroups();
            await SeedTeeth();

            // TO DO: Init data
        }

        private IList<TModel> getDeserializedModel<TModel>(string jsonFilePath)
        {
            JsonTextReader jsonReader = null;
            try
            {
                JsonSerializer jsonSerializer = new JsonSerializer
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Culture = System.Globalization.CultureInfo.InvariantCulture,
                    NullValueHandling = NullValueHandling.Ignore,
                    FloatFormatHandling = FloatFormatHandling.String
                };
                jsonReader = new JsonTextReader(new StreamReader(jsonFilePath));
                var models = jsonSerializer.Deserialize<IList<TModel>>(jsonReader);
                return models;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, "An error occurred on seed data serialization.");
                return null;
            }
            finally
            {
                if (jsonReader != null)
                    jsonReader.Close();
            }
        }

        private async Task SeedArea()
        {
            if (!_context.Countries.Any())
            {
                try
                {
                    var citiesData = getDeserializedModel<CitySeedModel>(ContentPaths.GetSeedDataPath("cities.json"));
                    var regionsData = getDeserializedModel<RegionSeedModel>(ContentPaths.GetSeedDataPath("regions.json"));
                    var countriesData = getDeserializedModel<CountrySeedModel>(ContentPaths.GetSeedDataPath("countries.json"));

                    if (citiesData != null
                        && citiesData.Count > 0
                        && regionsData != null
                        && regionsData.Count > 0
                        && countriesData != null
                        && countriesData.Count > 0)
                    {
                        List<Country> countries = new List<Country>();
                        countries.AddRange(countriesData.Select(s => new Country
                        {
                            Name = s.name,
                            Code = s.codeabc2,
                            Regions = s.code123 == "860"
                                        ? regionsData.Select(rg => new Region
                                        {
                                            Code = rg.codelat,
                                            Name = rg.nameRu ?? rg.nameUz ?? "",
                                            Cities = citiesData
                                            .Where(w => w.regionid.Equals(rg.regioncode))
                                            .Select(ct => new City
                                            {
                                                Code = ct.areacode,
                                                Name = ct.nameRu ?? ct.nameUz ?? "",
                                            }).ToList()
                                        }).ToList()
                                       : new List<Region>()

                        }));

                        _context.Countries.AddRange(countries);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed area data.");
                }
            }

        }

        private async Task SeedRoles()
        {
            try
            {
                if (!await _roleManager.RoleExistsAsync("admin"))
                    await _roleManager.CreateAsync(new Role { Name = "admin" });

                if (!await _roleManager.RoleExistsAsync("doctor"))
                    await _roleManager.CreateAsync(new Role { Name = "doctor" });

                if (!await _roleManager.RoleExistsAsync("patient"))
                    await _roleManager.CreateAsync(new Role { Name = "patient" });

                if (!await _roleManager.RoleExistsAsync("reception"))
                    await _roleManager.CreateAsync(new Role { Name = "reception" });

                if (!await _roleManager.RoleExistsAsync("cashier"))
                    await _roleManager.CreateAsync(new Role { Name = "cashier" });

                if (!await _roleManager.RoleExistsAsync("rentgen"))
                    await _roleManager.CreateAsync(new Role { Name = "rentgen" });
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, "An error occurred on seed roles data.");
            }
        }

        private async Task SeedUsers()
        {
            try
            {
                if (await _userManager.FindByNameAsync("admin") == null)
                {
                    User admin = new User
                    {
                        Email = "admin@edent.uz",
                        EmailConfirmed = true,
                        PhoneNumber = "998999999999",
                        PhoneNumberConfirmed = true,
                        UserName = "admin",
                        IsActive = true
                    };
                    await _userManager.CreateAsync(admin, "123admin");

                    if (!await _userManager.IsInRoleAsync(admin, "admin"))
                        await _userManager.AddToRoleAsync(admin, "admin");
                }

                if (await _userManager.FindByNameAsync("doctor") == null)
                {
                    User doctor = new User
                    {
                        Email = "doctor@edent.uz",
                        EmailConfirmed = true,
                        PhoneNumber = "998999999998",
                        PhoneNumberConfirmed = true,
                        UserName = "doctor",
                        IsActive = true
                    };
                    await _userManager.CreateAsync(doctor, "qwerty");
                    if (!await _userManager.IsInRoleAsync(doctor, "doctor"))
                        await _userManager.AddToRoleAsync(doctor, "doctor");
                }

                if (await _userManager.FindByNameAsync("reception") == null)
                {
                    User reception = new User
                    {
                        Email = "reception@edent.uz",
                        EmailConfirmed = true,
                        PhoneNumber = "998999999997",
                        PhoneNumberConfirmed = true,
                        UserName = "reception",
                        IsActive = true
                    };
                    await _userManager.CreateAsync(reception, "qwerty");
                    if (!await _userManager.IsInRoleAsync(reception, "reception"))
                        await _userManager.AddToRoleAsync(reception, "reception");
                }

                if (await _userManager.FindByNameAsync("cashier") == null)
                {
                    User cashier = new User
                    {
                        Email = "cashier@edent.uz",
                        EmailConfirmed = true,
                        PhoneNumber = "998999999996",
                        PhoneNumberConfirmed = true,
                        UserName = "cashier",
                        IsActive = true
                    };
                    await _userManager.CreateAsync(cashier, "qwerty");
                    if (!await _userManager.IsInRoleAsync(cashier, "cashier"))
                        await _userManager.AddToRoleAsync(cashier, "cashier");
                }
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError(ex, "An error occurred on seed users data.");
            }
        }

        private async Task SeedOrganizations()
        {
            if (!_context.Organizations.Any())
            {
                try
                {
                    City city = _cityService.GetByName("Яккасарайский район");
                    List<Organization> organizations = new List<Organization>
                    {
                        new Organization {
                            INN = "123456789",
                            MFO = "12345",
                            OKED = "12345",
                            OKONX = "12345",
                            Name = "Edent Medical",
                            Address = new Address
                            {
                                CityId = city.Id,
                                AddressLine1 = "Qushbegi",
                                AddressLine2 = "Parisien 10"
                            },
                            LogoImage = "dentos_logo_white.png"
                        }
                    };
                    _context.Organizations.AddRange(organizations);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed organizations data.");
                }
            }
        }

        private async Task SeedProfessions()
        {
            if (!_context.Professions.Any())
            {
                try
                {
                    var professionsData = getDeserializedModel<ProfessionSeedModel>(ContentPaths.GetSeedDataPath("professions.json"));

                    if (professionsData != null
                        && professionsData.Count > 0)
                    {
                        List<Profession> professions = new List<Profession>();
                        professions.AddRange(professionsData.Select(s => new Profession
                        {
                            Name = s.Name,
                            Description = s.Description,
                            Specializations = s.Specializations.Select(sp => new Specialization
                            {
                                Name = sp.Name,
                                Description = sp.Description,
                                DisplayOrder = sp.Order
                            }).ToList()
                        }));

                        _context.Professions.AddRange(professions);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed professions data.");
                }
            }

        }

        private async Task SeedTerms()
        {
            if (!_context.Terms.Any())
            {
                try
                {
                    List<Term> terms = new List<Term>()
                    {
                        new Term { Name = "Аренда", Type = Enums.TermType.Rent },
                        new Term { Name = "Процент", Type = Enums.TermType.Percent },
                        new Term { Name = "Фиксированный", Type=Enums.TermType.Fixed }
                    };

                    _context.Terms.AddRange(terms);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed professions data.");
                }
            }
        }

        private async Task SeedDoctors()
        {
            if (!_context.Doctors.Any())
            {
                try
                {
                    City city = _cityService.GetByName("Яккасарайский район");
                    Organization organization = _organizationService.GetByName("Edent Medical");
                    Term term = _termService.GetByName("Процент");

                    List<Doctor> doctors = new List<Doctor>();

                    var doctor = await _userManager.FindByNameAsync("doctor");
                    if (doctor != null)
                    {
                        Specialization specialization = _specializationService.GetByName("Стоматолог");
                        doctors.Add(new Doctor
                        {
                            BirthDate = new DateTime(1987, 12, 18),
                            FirstName = "Doctor",
                            LastName = "Doctorov",
                            Patronymic = "Doctorin",
                            DoctorAddresses = new List<DoctorAddress>
                            {
                                new DoctorAddress
                                {
                                    IsActive = true,
                                    Address = new Address
                                    {
                                        AddressLine1 = "Adress of Otabek",
                                        AddressLine2 = "Gde to za...",
                                        CityId = city.Id
                                    }
                                }
                            },
                            DoctorInTerms = new List<DoctorInTerm>
                            {
                                new DoctorInTerm
                                {
                                   TermId = term.Id,
                                   TermValue = 50
                                }
                            },
                            SpecializationId = specialization.Id,
                            OrganizationId = organization.Id,
                            UserId = doctor.Id,
                            Schedule = new Schedule
                            {
                                AdmissionDuration = 30,
                                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                                ToDate = new DateTime(DateTime.Now.Year, 12, 31),
                                IsActive = true,
                                ScheduleSettings = new HashSet<ScheduleSetting>
                                    {
                                        new ScheduleSetting
                                        {
                                            FromMinute = TimeSpan.FromMinutes(540),
                                            ToMinute = TimeSpan.FromMinutes(780),
                                            SettingDayOfWeeks = new HashSet<SettingDayOfWeek>
                                            {
                                                new SettingDayOfWeek { DayOfWeek = DayOfWeek.Monday },
                                                new SettingDayOfWeek { DayOfWeek = DayOfWeek.Wednesday },
                                                new SettingDayOfWeek { DayOfWeek = DayOfWeek.Friday }
                                            }
                                        },
                                        new ScheduleSetting
                                        {
                                            FromMinute =  TimeSpan.FromMinutes(840),
                                            ToMinute =  TimeSpan.FromMinutes(1080),
                                            SettingDayOfWeeks = new HashSet<SettingDayOfWeek>
                                            {
                                                new SettingDayOfWeek { DayOfWeek = DayOfWeek.Tuesday },
                                                new SettingDayOfWeek { DayOfWeek = DayOfWeek.Thursday },
                                                new SettingDayOfWeek { DayOfWeek = DayOfWeek.Saturday }
                                            }
                                        }
                                    }
                            }
                        });
                    }

                    _context.Doctors.AddRange(doctors);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed doctors data.");
                }
            }
        }

        private async Task SeedEmployees()
        {
            if (!_context.Employees.Any())
            {
                try
                {
                    List<Employee> employees = new List<Employee>();

                    var reception = await _userManager.FindByNameAsync("reception");
                    if (reception != null)
                    {
                        employees.Add(new Employee
                        {
                            BirthDate = new DateTime(1987, 12, 18),
                            FirstName = "Reception",
                            LastName = "Receptionov",
                            Patronymic = "Receptionin",
                            UserId = reception.Id
                        });
                    }

                    var cashier = await _userManager.FindByNameAsync("cashier");
                    if (cashier != null)
                    {
                        employees.Add(new Employee
                        {
                            BirthDate = new DateTime(1987, 12, 18),
                            FirstName = "Cashier",
                            LastName = "Cashierov",
                            Patronymic = "Cashierin",
                            UserId = cashier.Id
                        });
                    }

                    var admin = await _userManager.FindByNameAsync("admin");
                    if (admin != null)
                    {
                        employees.Add(new Employee
                        {
                            BirthDate = new DateTime(1987, 12, 18),
                            FirstName = "Admin",
                            LastName = "Adminov",
                            Patronymic = "Adminin",
                            UserId = admin.Id
                        });
                    }

                    _context.Employees.AddRange(employees);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed doctors data.");
                }
            }
        }

        private async Task SeedMeasurementUnitTypes()
        {
            if (!_context.MeasurementUnitTypes.Any())
            {
                try
                {
                    var measurementUnitTypeData = getDeserializedModel<MeasurementUnitTypeViewModel>(ContentPaths.GetSeedDataPath("measurementunit.json"));
                    var measurementUnitTypes = _mapper.Map<IList<MeasurementUnitType>>(measurementUnitTypeData);
                    _context.MeasurementUnitTypes.AddRange(measurementUnitTypes);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed measurement units data.");
                }
            }
        }

        private async Task SeedInventories()
        {
            if (!_context.Inventories.Any())
            {
                try
                {
                    var inventoryData = getDeserializedModel<InventoryViewModel>(ContentPaths.GetSeedDataPath("inventory.json"));
                    var measurementUnits = _measurementUnitService.GetAll().ToList();
                    List<Inventory> inventories = new List<Inventory>();
                    foreach (var item in inventoryData)
                    {
                        var inventory = _mapper.Map<Inventory>(item);
                        var mUnitType = measurementUnits.FirstOrDefault(f => f.MeasurementUnits.Any(a => a.Code == item.MesurementUnitCode));
                        if (mUnitType == null)
                            continue;

                        var mUnit = mUnitType.MeasurementUnits.FirstOrDefault(f => f.Code == item.MesurementUnitCode);
                        if (mUnit != null)
                        {
                            inventory.Stock = mUnit.Multiplicity * inventory.Stock;
                        }
                        inventory.MeasurementUnitId = mUnit.Id;
                        inventory.MeasurementUnitTypeId = mUnitType.Id;
                        inventories.Add(inventory);
                    }
                    _context.Inventories.AddRange(inventories);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed inventories units data.");
                }
            }
        }

        private async Task SeedDentalServiceCategories()
        {
            if (!_context.DentalServiceCategories.Any())
            {
                try
                {
                    var categoriesData = getDeserializedModel<DentalServiceCategoryViewModel>(ContentPaths.GetSeedDataPath("dentalservicecategories.json"));
                    var categories = _mapper.Map<IList<DentalServiceCategory>>(categoriesData);
                    _context.DentalServiceCategories.AddRange(categories);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed dental service categories data.");
                }
            }
        }

        private async Task SeedDentalServiceGroups()
        {
            if (!_context.DentalServiceGroups.Any())
            {
                try
                {
                    var dentalServiceCategories = _context.DentalServiceCategories.ToList();
                    var groupsData = getDeserializedModel<DentalServiceGroupViewModel>(ContentPaths.GetSeedDataPath("dentalservicegroups.json"));
                    foreach (var item in groupsData)
                    {
                        foreach (var ds in item.DentalServices)
                        {
                            var category = dentalServiceCategories.FirstOrDefault(f => f.Name == ds.CategoryName);
                            if (category != null)
                            {
                                ds.DentalServiceCategoryId = category.Id;
                            }
                        }
                        var group = _mapper.Map<DentalServiceGroup>(item);
                        _context.DentalServiceGroups.Add(group);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed dental service groups data.");
                }
            }
        }

        private async Task SeedTeeth()
        {
            if (!_context.Teeth.Any())
            {
                try
                {
                    var toothData = getDeserializedModel<ToothViewModel>(ContentPaths.GetSeedDataPath("teeth.json"));
                    var teeth = _mapper.Map<IList<Tooth>>(toothData);
                    await _context.AddRangeAsync(teeth);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (_logger != null)
                        _logger.LogError(ex, "An error occurred on seed tooth data.");
                }
            }
        }

    }
}
