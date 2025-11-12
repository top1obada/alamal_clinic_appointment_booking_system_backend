using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.PersonDTO
{
    public class clsPersonDTO
    {
        public int? PersonID { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public char? Gender { get; set; }
        public string? Nationality { get; set; }
    }
}
