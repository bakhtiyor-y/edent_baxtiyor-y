using Edent.Api.Infrastructure.Enums;
using Edent.Api.Infrastructure.Filters;
using Edent.Api.Services.EntityServices.Interfaces;
using Edent.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeNG.TableFilter;
using PrimeNG.TableFilter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IUserResolverService _userResolverService;
        private readonly IPatientToothService _patientToothService;
        private readonly ITreatmentDentalServiceService _treatmentDentalServiceService;

        public PatientController(IPatientService patientService,
            IDoctorService doctorService,
            IUserResolverService userResolverService,
            IPatientToothService patientToothService,
            ITreatmentDentalServiceService treatmentDentalServiceService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _userResolverService = userResolverService;
            _patientToothService = patientToothService;
            _treatmentDentalServiceService = treatmentDentalServiceService;
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var doctor = _patientService
                .Query()
                .Include("PatientAddresses.Address.City.Region")
                .FirstOrDefault(f => f.Id == id);

            var result = _patientService.Mapper.Map<PatientManageViewModel>(doctor);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string filter, [FromQuery] string name = null)
        {
            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _patientService
                .Query()
                .Include("PatientAddresses.Address.City.Region");

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lname = name.ToLower();
                query = query.Where(w => w.FirstName.ToLower().Contains(lname) || w.LastName.ToLower().Contains(lname));
            }

            query = query
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _patientService.Mapper.Map<IEnumerable<PatientViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpGet("GetDoctorPatients")]
        public IActionResult GetDoctorPatients([FromQuery] string filter, [FromQuery] string name = null)
        {
            var userId = _userResolverService.CurrentUserId.HasValue ? _userResolverService.CurrentUserId.Value : 0;
            if (userId == 0)
            {
                return BadRequest(new JsonErrorResponse { Messages = new[] { "User not found" } });
            }

            var doctor = _doctorService
                .Query()
                .FirstOrDefault(f => f.UserId == userId);
            if (doctor == null)
                return BadRequest(new JsonErrorResponse { Messages = new[] { "Doctor not found!" } });

            var filterModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TableFilterModel>(filter);
            var query = _patientService
                .Query()
                .Include("PatientAddresses.Address.City.Region")
                .Where(w => w.Appointments.Any(a => a.DoctorId == doctor.Id));

            if (!string.IsNullOrWhiteSpace(name))
            {
                string lname = name.ToLower();
                query = query.Where(w => w.FirstName.ToLower().Contains(lname) || w.LastName.ToLower().Contains(lname));
            }

            query = query
                .OrderBy(o => o.Id)
                .PrimengTableFilter(filterModel, out int totalRecord);

            var result = _patientService.Mapper.Map<IEnumerable<PatientViewModel>>(query);
            return Ok(new { Data = result, Total = totalRecord });
        }

        [HttpPost]
        public IActionResult Post(PatientManageViewModel viewModel)
        {
            var result = _patientService.CreatePatient(viewModel);
            _patientToothService.InitPatientTeeth(result.Id, viewModel.PatientAgeType);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(PatientManageViewModel viewModel)
        {
            var result = await _patientService.UpdatePatient(viewModel);
            _patientToothService.InitPatientTeeth(result.Id, viewModel.PatientAgeType);
            return Ok(result);
        }

        [HttpGet("SearchByName")]
        public IActionResult SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Ok(new PatientManageViewModel[0]);

            name = name.ToLower();

            var query = _patientService
                .Query()
                .Include("PatientAddresses.Address.City.Region")
                .Where(w => w.FirstName.ToLower().StartsWith(name) || w.LastName.ToLower().StartsWith(name));

            var result = _patientService.Mapper.Map<IEnumerable<PatientManageViewModel>>(query);
            return Ok(result);
        }

        [HttpGet("GetTeeth/{patientId}")]
        public IActionResult GetTeeth(int patientId)
        {

            var patient = _patientService.GetById(patientId);
            var toothType = patient.PatientAgeType == PatientAgeType.Adult ? ToothType.Adult : ToothType.Child;
            var query = _patientToothService
                .Query()
                .Include(i => i.Tooth)
                .Where(w => w.PatientId == patientId)
                .ToList();

            var patientTeeth = _patientService.Mapper.Map<IEnumerable<PatientToothViewModel>>(query).Where(w => w.Tooth.ToothType == toothType);

            PatientTeethViewModel result = new();
            result.PatientId = patientId;
            result.PatientAgeType = patient.PatientAgeType;
            result.TopLeft = patientTeeth.Where(w => w.Tooth.Direction == ToothDirection.TopLeft).OrderByDescending(o => o.Tooth.Position).ToList();
            result.TopRight = patientTeeth.Where(w => w.Tooth.Direction == ToothDirection.TopRight).OrderBy(o => o.Tooth.Position).ToList();
            result.BottomLeft = patientTeeth.Where(w => w.Tooth.Direction == ToothDirection.BottomLeft).OrderByDescending(o => o.Tooth.Position).ToList();
            result.BottomRight = patientTeeth.Where(w => w.Tooth.Direction == ToothDirection.BottomRight).OrderBy(o => o.Tooth.Position).ToList();

            return Ok(result);
        }

        [HttpGet("GetPatientToothHistory")]
        public IActionResult GetPatientToothHistory(int patientToothId)
        {
            var result = _treatmentDentalServiceService
                .Query()
                .Include(i => i.DentalService)
                .Include(i => i.Treatment)
                .Where(w => w.Treatment.PatientToothId == patientToothId)
                .Select(s => new
                {
                    TreatmentDate = s.Treatment.CreatedDate,
                    DentalServiceName = s.DentalService.Name,
                    Description = s.Treatment.Description
                })
                .OrderBy(o => o.TreatmentDate)
                .ToList();
            return Ok(result);
        }
    }
}
