using Assignment_DotNet6.Entities;
using Assignment_DotNet6.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_DotNet6.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class HospitalController : ControllerBase
    {

        private readonly ILogger<HospitalController> _logger;
        private DBService dbService;

        public HospitalController(ILogger<HospitalController> logger, DBService auth)
        {
            _logger = logger;
            this.dbService = auth;
        }
     
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDetails model)
        {
            _logger.LogInformation("asjdaskldjhaskjfhasdkfjhakldfhaksdjfhkalsjdfhaskdjfhasjkldfhaskldfhaskldfhd");
            var resp = await dbService.AuthenticateUser(model);
            if (resp != null && resp.AuthenticationStatus == Entities.UserAuthenticationStatus.Authenticated)
            {
                return Ok(resp);
            }
            return Unauthorized("Invalid Credentials!");
        }


        #region Create
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("CreateDoctor")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.CreateDoctor(doctor));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("CreateVisit")]
        public async Task<IActionResult> CreateVisit(Visit Visit)
        {
            if (Visit == null)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.CreateVisit(Visit));
        }

        #endregion

        #region Read
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetDoctorById")]
        public async Task<IActionResult> GetDoctorById(Guid guid)
        {
            return Ok(await dbService.GetDoctorById(guid));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetPatientById")]
        public async Task<IActionResult> GetPatientById(Guid guid)
        {
            return Ok(await dbService.GetPatientById(guid));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            return Ok(await dbService.GetAllDoctors());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetAllPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            return Ok(await dbService.GetAllPatients());
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetAllVisit")]
        public async Task<IActionResult> GetAllVisit()
        {
            return Ok(await dbService.GetAllVisit());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetVisitsByPatientId")]
        public async Task<IActionResult> GetVisitsByPatientId(Guid Guid)
        {
            return Ok(await dbService.GetVisitsByPatientId(Guid));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetVisitsByDoctorId")]
        public async Task<IActionResult> GetVisitsByDoctorId(Guid Guid)
        {
            return Ok(await dbService.GetVisitsByDoctorId(Guid));
        }

        #endregion

        #region Update
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("UpdateDoctorById")]
        public async Task<IActionResult> UpdateDoctorById(Doctor Doctor)
        {
            if (Doctor == null)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.UpdateDoctorById(Doctor));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(Patient Patient)
        {
            if (Patient == null)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.UpdatePatientById(Patient));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("UpdateVisitById")]
        public async Task<IActionResult> UpdateVisitById(Visit Visit)
        {
            if (Visit == null)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.UpdateVisitById(Visit));
        }


        #endregion

        #region Delete
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctor(Guid Guid)
        {
            if (Guid == Guid.Empty)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.DeleteDoctorById(Guid));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("DetelePatient")]
        public async Task<IActionResult> DetelePatient(Guid Guid)
        {
            if (Guid == Guid.Empty)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.DeletePatientById(Guid));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("DeleteVisitById")]
        public async Task<IActionResult> DeleteVisitById(Guid Guid)
        {
            if (Guid == Guid.Empty)
            {
                return BadRequest("Please fill in Information");
            }
            return Ok(await dbService.DeleteVisitById(Guid));
        }
        #endregion









    }
}
