using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentBookingBussinessTier.Object.Patient.Services;
using ClinicAppointmentBookingBussinessTier.Object.Patient.Services.SaveCompletedPatient;
using ClinicAppointmentsBookingDTO.PatientDTO;
using ClinicAppointmentsBookingDTO.DoctorDTO;
using ClinicAppointmentsBookingDTO.UserDTO;
using ClinicAppointmentsBookingDTO.SessionDTO;
using ClinicAppointmentBookingBussinessTier.Object.Session.Services;
using ProjectsServices.RefreshTokenServices;
using System.Security.Claims;
using ProjectsServices.JWTServices;
using Microsoft.AspNetCore.Authorization;
namespace ClinicAppointmentBooking_InterfacesAPIs.Controllers.Patient
{



    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {


        private IConfiguration _configuration;


        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("GetPatientInfo/{patientID}")]
        [Authorize]
        [ProducesResponseType(typeof(clsPatientInfoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientInfo(int patientID)
        {
            if (patientID <= 0)
                return BadRequest("Invalid patient ID");

            var service = new clsGetPatientInfoService();
            var patientInfo = await service.Get(patientID);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, service.ErrorMessage );

            if (patientInfo == null)
                return NotFound("Patient not found");

            return Ok(patientInfo);
        }



        [HttpGet("GetAllPatients/{pageSize}/{pageNumber}")]
        [Authorize(Roles = nameof(enUserRole.eReceptionist))]
        [ProducesResponseType(typeof(List<clsClinicPatientDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPatients(int pageSize, int pageNumber)
        {
            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetPatientsService();
            var patients = await service.Get(pageSize, pageNumber);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500,service.ErrorMessage);

            return Ok(patients);
        }




        [HttpPost]
        [ProducesResponseType(typeof(clsRetrivingAddPatientDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPatient(
             [FromBody] clsCompletedPatientDTO CompletedPatientDTO,
             [FromHeader(Name = "LoginData")] string LoginRequirementJson)
        {
            if (CompletedPatientDTO == null)
                return BadRequest("Patient data is required");

            if (CompletedPatientDTO.Person == null)
                return BadRequest("Person information is required");

            if (CompletedPatientDTO.User == null)
                return BadRequest("User information is required");

            if (CompletedPatientDTO.Patient == null)
                return BadRequest("Patient information is required");

            if (string.IsNullOrWhiteSpace(LoginRequirementJson))
                return BadRequest("Login requirements are required");

            clsLoginRequirementsDTO loginRequirements;
            try
            {
                loginRequirements = System.Text.Json.JsonSerializer.Deserialize<clsLoginRequirementsDTO>(LoginRequirementJson);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid login requirements format");
            }

            if (string.IsNullOrWhiteSpace(loginRequirements?.UserName))
                return BadRequest("Username is required");

            if (string.IsNullOrWhiteSpace(loginRequirements?.Password))
                return BadRequest("Password is required");

            var completedPatientWithLogin = new CompletedPatientWithLoginRequirements
            {
                CompletedPatientDTO = CompletedPatientDTO,
                LoginRequirmentsDTO = loginRequirements
            };

            var service = new clsSaveCompletedPatientService();
            var success = await service.Save(completedPatientWithLogin);

            if (!success)
                return StatusCode(500, service.ErrorMessage);

            

            string NativeRefreshToken = clsRefreshTokenHelper.GenerateRefreshToken();

            string HashedRefreshToken = clsRefreshTokenHelper.HashToken(NativeRefreshToken);


            var Session = new clsInsertSessionDTO()
            {
                UserID = CompletedPatientDTO.User.UserID,
                HashedRefreshToken = HashedRefreshToken,
            };

            var SaveSessionService = new clsSaveSessionService();

            var SaveSessionResult = await SaveSessionService.Save(Session);

            if (SaveSessionResult)
            {

                var Claims = new Claim[]
                {
                new Claim ("UserBranchID",CompletedPatientDTO.Patient.PatientID.ToString()),
                new Claim (ClaimTypes.Role,nameof(enUserRole.ePatient)),
                new Claim ("PersonID",CompletedPatientDTO.Person.PersonID.ToString()),
                new Claim ("FirstName",CompletedPatientDTO.Person.FirstName.ToString()),
                new Claim ("UserID",CompletedPatientDTO.User.UserID.ToString()),
                new Claim ("JoiningDate",CompletedPatientDTO.User.JoiningDate.ToString())
                };

                var JWTToken = clsJWTHelper.GenerateJwtToken(Claims, clsJWTHelper.GetToken(_configuration));

                return Ok(new clsUserTokens() { JWTToken = JWTToken, RefreshToken = NativeRefreshToken });
            }


            return StatusCode(500, SaveSessionService.ErrorMessage);
            

            
          
        }



        [HttpGet("GetPatientDetails/{patientID}")]
        [Authorize]
        [ProducesResponseType(typeof(clsPatientDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientDetails(int patientID)
        {
            if (patientID <= 0)
                return BadRequest("Invalid patient ID");

            var service = new clsGetPatientDetailsService();
            var patientDetails = await service.Get(patientID);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, service.ErrorMessage);

            if (patientDetails == null)
                return NotFound("Patient not found");

            return Ok(patientDetails);
        }


    }
}