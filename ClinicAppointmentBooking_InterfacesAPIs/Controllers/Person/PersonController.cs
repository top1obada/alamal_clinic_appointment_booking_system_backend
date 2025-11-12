using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicAppointmentBookingBussinessTier.Object.Person;
using ClinicAppointmentBookingBussinessTier.Object.Person.Services;
using ClinicAppointmentsBookingDTO.PersonDTO;
using Microsoft.AspNetCore.Authorization;
using ClinicAppointmentsBookingDTO.UserDTO;

namespace ClinicAppointmentBooking_InterfacesAPIs.Controllers.Person
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet("{personID}")]
        [Authorize]
        [ProducesResponseType(typeof(clsPersonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPerson(int personID)
        {
            if (personID <= 0)
                return BadRequest("Invalid person ID");

            var person = await clsPerson.Find(personID);

            if (person == null)
                return NotFound("Person not found");

            return Ok(person.PersonDTO);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePerson([FromBody] clsPersonDTO personDTO)
        {

            var Value = User.FindFirst("PersonID");

            if (Value == null) return BadRequest("There Is No PersonID");

            int PersonID = Convert.ToInt32(Value.Value);

            if (personDTO == null)
                return BadRequest("Person data is required");

            if (PersonID <= 0)
                return BadRequest("Invalid person ID");


            var existingPerson = await clsPerson.Find(PersonID);
            if (existingPerson == null)
                return NotFound("Person not found");

           
            existingPerson.PersonDTO = personDTO;

            var service = new clsUpdatePersonService();
            var success = await service.Save(existingPerson.PersonDTO);

            if (!success)
                return StatusCode(500, service.ErrorMessage);

            return Ok();
        }
    }
}