using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentsBookingDTO.PersonDTO;
using ClinicAppointmentsBookingDTO.UserDTO;

namespace ClinicAppointmentsBookingDTO.PatientDTO
{
    public class clsCompletedPatientDTO
    {
        public clsPersonDTO? Person { get; set; }
        public clsUserDTO? User { get; set; }
        public clsPatientDTO? Patient { get; set; }
    }
}
