using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentBookingBussinessTier.Object.Doctor.Services;
using Microsoft.AspNetCore.Authorization;

namespace ClinicAppointmentBooking_InterfacesAPIs.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSpecialtyController : ControllerBase
    {
        [HttpGet("GetAll")]
        [Authorize]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDoctorSpecialties()
        {
            var service = new clsGetDoctorSpecialtiesService();
            var specialties = await service.Get();

            if (specialties == null)
                return StatusCode(500, service.ErrorMessage);

            return Ok(specialties);
        }
    }
}