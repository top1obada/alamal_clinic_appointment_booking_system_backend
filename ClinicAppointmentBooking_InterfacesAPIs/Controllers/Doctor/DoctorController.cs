using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentBookingBussinessTier.Object.Doctor.Services;
using ClinicAppointmentsBookingDTO.DoctorDTO;

using ClinicAppointmentBookingBussinessTier.Object.Doctor.Services.SaveCompletedDoctor;
using ClinicAppointmentsBookingDTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using ClinicAppointmentBookingBussinessTier.Object.Doctor.Services.UpdateCompletedDoctor;
using ClinicAppointmentBookingBussinessTier.Object.Doctor.Services.FindDoctor;

namespace ClinicAppointmentBooking_InterfacesAPIs.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {



        [HttpGet("GetAllDoctors/{pageSize}/{pageNumber}")]
        [Authorize]
        [ProducesResponseType(typeof(List<clsClinicDoctorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDoctors(int pageSize, int pageNumber)
        {
            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetClinicDoctorsService();
            var doctors = await service.Get(pageSize, pageNumber);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, new { error = service.ErrorMessage });

            return Ok(doctors);
        }






        [HttpGet("GetDoctorsBySpecialty/{specialtyName}/{pageSize}/{pageNumber}")]
        [Authorize]
        [ProducesResponseType(typeof(List<clsClinicDoctorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctorsBySpecialty(string specialtyName, int pageSize, int pageNumber)
        {
            if (specialtyName == "null") specialtyName = null;

            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetClinicDoctorsBySpecialNameService();
            var doctors = await service.Get(pageNumber, pageSize, specialtyName);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, new { error = service.ErrorMessage });

            return Ok(doctors);
        }






        [HttpGet("GetDoctorDetails/{doctorID}")]
        [Authorize]
        [ProducesResponseType(typeof(clsDoctorDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctorDetails(int doctorID)
        {
            if (doctorID <= 0)
                return BadRequest("Invalid doctor ID");

            var service = new clsGetDoctorDetailsService();
            var doctorDetails = await service.Get(doctorID);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, new { error = service.ErrorMessage });

            if (doctorDetails == null)
                return NotFound("Doctor not found");

            return Ok(doctorDetails);
        }






        [HttpPost]
        [Authorize(Roles = nameof(enUserRole.eReceptionist))]
        [ProducesResponseType(typeof(clsRetrivingAddDoctorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDoctor(
                [FromBody] clsCompletedDoctorDTO doctorData,
                [FromHeader] string LoginRequirementJson)
        {
            if (doctorData == null)
                return BadRequest("Doctor data is required");

            if (doctorData.Person == null)
                return BadRequest("Person information is required");

            if (doctorData.User == null)
                return BadRequest("User information is required");

            if (doctorData.Doctor == null)
                return BadRequest("Doctor information is required");

            if (doctorData.DoctorConsultation == null)
                return BadRequest("Consultation information is required");

            if (string.IsNullOrWhiteSpace(LoginRequirementJson))
                return BadRequest("Login requirements are required");

            clsLoginRequirementsDTO loginRequirements = null;
            try
            {
                loginRequirements = System.Text.Json.JsonSerializer.Deserialize<clsLoginRequirementsDTO>(LoginRequirementJson);
            }
            catch (Exception)
            {
                return BadRequest("Invalid login requirements format");
            }

            if (string.IsNullOrWhiteSpace(loginRequirements?.UserName))
                return BadRequest("Username is required");

            if (string.IsNullOrWhiteSpace(loginRequirements?.Password))
                return BadRequest("Password is required");

            var completedDoctorWithLogin = new CompletedDoctorWithLoginRequirements
            {
                CompletedDoctorDTO = doctorData,
                LoginRequirmentsDTO = loginRequirements
            };

            var service = new clsSaveCompletedDoctorService();



            var success = await service.Save(completedDoctorWithLogin);

            if (!success)
                return StatusCode(500, service.ErrorMessage);

            return Ok(new clsRetrivingAddDoctorDTO()
            {
                DoctorID = doctorData.Doctor.DoctorID,
                UserID = doctorData.User.UserID,
                PersonID = doctorData.Person.PersonID,
                DoctorConsultationID = doctorData.DoctorConsultation.DoctorConsultationID
            });
        }


        [HttpGet("SearchByName/{doctorName}/{pageSize}/{pageNumber}")]
        [Authorize]
        [ProducesResponseType(typeof(List<clsClinicDoctorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchDoctorsByName(string doctorName, int pageSize, int pageNumber)
        {
            if (string.IsNullOrWhiteSpace(doctorName))
                return BadRequest("Doctor name is required");

            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetClinicDoctorsByNameService();
            var doctors = await service.Get(pageNumber, pageSize, doctorName);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, new { error = service.ErrorMessage });

            return Ok(doctors);
        }

        [HttpGet("GetPreviousDoctorsAppointments/{patientID}/{pageSize}/{pageNumber}")]
        [Authorize]
        [ProducesResponseType(typeof(List<clsClinicDoctorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPreviousDoctorsAppointments(int patientID, int pageSize, int pageNumber)
        {
            if (patientID <= 0)
                return BadRequest("Invalid patient ID");

            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetClinicDoctorsPreviousAppointmentService();
            var doctors = await service.Get(pageNumber, pageSize, patientID);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, new { error = service.ErrorMessage });

            return Ok(doctors);
        }



        [HttpGet("GetPatientFavoriteDoctors/{patientID}/{pageSize}/{pageNumber}")]
        [Authorize]
        [ProducesResponseType(typeof(List<clsClinicDoctorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientFavoriteDoctors(int patientID, int pageSize, int pageNumber)
        {
            if (patientID <= 0)
                return BadRequest("Invalid patient ID");

            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetClinicDoctorsPatientFavoriteService();
            var doctors = await service.Get(pageNumber, pageSize, patientID);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, new { error = service.ErrorMessage });

            return Ok(doctors);
        }


        [HttpPut("UpdateDoctor")]
        [Authorize(Roles = nameof(enUserRole.eReceptionist))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDoctor([FromBody] clsCompletedDoctorDTO doctorData)
        {
            if (doctorData == null)
                return BadRequest("Doctor data is required");

            if (doctorData.Doctor?.DoctorID == null || doctorData.Doctor.DoctorID <= 0)
                return BadRequest("Valid Doctor ID is required");

            if (doctorData.Person == null)
                return BadRequest("Person information is required");

            if (doctorData.DoctorConsultation == null)
                return BadRequest("Consultation information is required");

            var service = new clsUpdateCompletedDoctorService();
            var success = await service.Update(doctorData);

            if (!success)
            {
                if (service.ErrorMessage?.Contains("not found") == true)
                    return NotFound(service.ErrorMessage);

                return StatusCode(500, new { error = service.ErrorMessage });
            }

            return Ok(new { message = "Doctor updated successfully" });
        }




        [HttpGet("FindDoctor/{doctorID}")]
        [Authorize(Roles = nameof(enUserRole.eReceptionist))]
        [ProducesResponseType(typeof(clsCompletedDoctorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindDoctor(int doctorID)
        {
            if (doctorID <= 0)
                return BadRequest("Invalid doctor ID");

            var service = new clsFindDoctorService();
            var doctor = await service.Get(doctorID);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
            {
                if (service.ErrorMessage.Contains("not found") || doctor == null)
                    return NotFound("Doctor not found");

                return StatusCode(500, new { error = service.ErrorMessage });
            }

            return Ok(doctor);
        }





    }
}