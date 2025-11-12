using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentBookingBussinessTier.Object.Appointment.RejectingCause;
using ClinicAppointmentBookingBussinessTier.Object.Appointment.RejectingCause.Services;
using ClinicAppointmentsBookingDTO.AppointmentDTO.RejectingCauseDTO;
using Microsoft.AspNetCore.Authorization;
using ClinicAppointmentsBookingDTO.UserDTO;

namespace ClinicAppointmentBooking_InterfacesAPIs.Controllers.Appointment
{
    [Route("api/[controller]")]
    [ApiController]
    public class RejectingCauseController : ControllerBase
    {
        [HttpGet("GetRejectingCause/{appointmentID}")]
        [Authorize]
        [ProducesResponseType(typeof(clsRejectingCause), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRejectingCause(int appointmentID)
        {
            if (appointmentID <= 0)
                return BadRequest("Invalid appointment ID");

            var rejectingCause = await clsRejectingCause.Find(appointmentID);

            if (rejectingCause == null)
                return NotFound("Rejecting cause not found for this appointment");

            return Ok(rejectingCause);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(enUserRole.eReceptionist)},{nameof(enUserRole.eDoctor)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRejectingCause([FromBody] clsRejectingCauseDTO rejectingCauseDTO)
        {
            if (rejectingCauseDTO == null)
                return BadRequest("Rejecting cause data is required");

            if (rejectingCauseDTO.AppointmentID <= 0)
                return BadRequest("Invalid appointment ID");

            if (string.IsNullOrWhiteSpace(rejectingCauseDTO.Text))
                return BadRequest("Rejection text is required");

            var service = new clsSaveRejectingCauseService();
            var RejectingCause = new clsRejectingCause() { RejectingCauseDTO = rejectingCauseDTO };

            var success = await service.Save(RejectingCause);

            if (!success)
                return StatusCode(500, service.ErrorMessage);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = $"{nameof(enUserRole.eReceptionist)},{nameof(enUserRole.eDoctor)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRejectingCause([FromBody] clsRejectingCauseDTO rejectingCauseDTO)
        {
            if (rejectingCauseDTO == null)
                return BadRequest("Rejecting cause data is required");

            if (rejectingCauseDTO.AppointmentID <= 0)
                return BadRequest("Invalid appointment ID");

            if (string.IsNullOrWhiteSpace(rejectingCauseDTO.Text))
                return BadRequest("Rejection text is required");

            var rejectingCause = await clsRejectingCause.Find((int)rejectingCauseDTO.AppointmentID);

            if (rejectingCause == null)
                return NotFound("Rejecting cause not found for this appointment");

            rejectingCause.RejectingCauseDTO = rejectingCauseDTO;

            var service = new clsSaveRejectingCauseService();
            var success = await service.Save(rejectingCause);

            if (!success)
                return StatusCode(500, service.ErrorMessage);

            return Ok();
        }
    }
}