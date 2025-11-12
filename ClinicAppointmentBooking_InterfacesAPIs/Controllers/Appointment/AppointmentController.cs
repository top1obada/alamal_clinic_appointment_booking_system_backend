using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentBookingBussinessTier.Object.Appointment.Services;
using ClinicAppointmentBookingBussinessTier.Object.Patient.Services;
using ClinicAppointmentsBookingDTO.AppointmentDTO;
using ClinicAppointmentsBookingDTO.PatientDTO;
using Microsoft.AspNetCore.Authorization;
using ClinicAppointmentsBookingDTO.UserDTO;
using ClinicAppointmentBooking_InterfacesAPIs.AuthorizationActionsFilter;

namespace ClinicAppointmentBooking_InterfacesAPIs.Controllers.Appointment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        [HttpGet("GetClinicAppointments/{pageSize}/{pageNumber}")]
        [Authorize(Roles = nameof(enUserRole.eReceptionist))]
        [ProducesResponseType(typeof(List<clsClinicAppointmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClinicAppointments(int pageSize, int pageNumber)
        {
            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetClinicAppointmentsService();
            var appointments = await service.Get(pageSize, pageNumber);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, service.ErrorMessage);

            return Ok(appointments);
        }

        [HttpGet("GetPatientAppointments/{patientID}/{pageSize}/{pageNumber}")]
        [Authorize(Roles = $"{nameof(enUserRole.ePatient)},{nameof(enUserRole.eReceptionist)}")]
        [ProducesResponseType(typeof(List<clsPatientAppointmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientAppointments(int patientID, int pageSize, int pageNumber)
        {
            if (patientID <= 0)
                return BadRequest("Invalid patient ID");

            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetPatientAppointmentsService();
            var appointments = await service.Get(pageNumber, pageSize, patientID);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, service.ErrorMessage);

            return Ok(appointments);
        }



        [HttpGet("GetPatientAppointmentsByStatus/{patientID}/{appointmentStatus}/{pageSize}/{pageNumber}")]
        [Authorize(Roles = $"{nameof(enUserRole.ePatient)},{nameof(enUserRole.eReceptionist)}")]
        [ProducesResponseType(typeof(List<clsPatientAppointmentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPatientAppointmentsByStatus(int patientID, enAppointmentStatus appointmentStatus, int pageSize, int pageNumber)
        {
            if (patientID <= 0)
                return BadRequest("Invalid patient ID");

            if (pageSize <= 0 || pageNumber <= 0)
                return BadRequest("Page size and page number must be greater than 0");

            var service = new clsGetPatientAppointmentsByAppointmentStatusService();
            var statusDTO = new clsPatientAppointmentStatusDTO
            {
                PatientID = patientID,
                AppointmentStatus = appointmentStatus
            };

            var appointments = await service.Get(pageNumber, pageSize, statusDTO);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, service.ErrorMessage);

            return Ok(appointments);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(enUserRole.ePatient)},{nameof(enUserRole.eReceptionist)}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAppointment([FromBody] clsAppointmentDTO appointmentDTO)
        {
            if (appointmentDTO == null)
                return BadRequest("Appointment data is required");

            if (appointmentDTO.PatientID <= 0)
                return BadRequest("Invalid patient ID");

            if (appointmentDTO.DoctorID <= 0)
                return BadRequest("Invalid doctor ID");

            if (appointmentDTO.AppointmentTime == null)
                return BadRequest("Appointment time is required");

            var service = new clsSaveAppointmentService();
            var success = await service.Save(appointmentDTO);

            if (!success)
                return StatusCode(500, service.ErrorMessage);

            return Ok(appointmentDTO.AppointmentID);
        }



        [HttpPut("ChangeAppointmentStatus")]
        [Authorize]
        [ChangeAppointmentAuthorizationActionFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task <IActionResult> 
            ChangeAppointmentStatus([FromBody] clsChangeAppointmentStatusDTO changeStatusDTO)
        {
            if (changeStatusDTO == null)
                return BadRequest("Status change data is required");

            if (changeStatusDTO.AppointementID <= 0)
                return BadRequest("Invalid appointment ID");

            if (changeStatusDTO.AppointmentStatus == null)
                return BadRequest("Appointment status is required");

            var service = new clsChangeAppointmentStatusService();
            var success = await service.Save(changeStatusDTO);

            if (!success)
                return StatusCode(500, service.ErrorMessage);

            return Ok("Appointment status updated successfully");
        }

        [HttpPost("GetAvailableAppointmentTimes")]
        [Authorize]
        [ProducesResponseType(typeof(List<clsAvailableAppointmentTimeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailableAppointmentTimes([FromBody] clsAvailableTimeAppointmentsFilterDTO filterDTO)
        {
            if (filterDTO.DoctorID <= 0)
                return BadRequest("Invalid doctor ID");

            if (filterDTO.Date == null || filterDTO.Date < DateTime.Today)
                return BadRequest("Invalid date");

            var service = new clsGetAvailableAppointmentTimesService();
            var availableTimes = await service.Get(filterDTO);

            if (!string.IsNullOrEmpty(service.ErrorMessage))
                return StatusCode(500, new { error = service.ErrorMessage });

            return Ok(availableTimes);
        }

    }
}