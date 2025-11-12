using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.PatientDTO
{
    public class clsPatientInfoDTO
    {
        public int? PersonID { get; set; }
        public string? PatientName { get; set; }
        public DateTime? BirthDate { get; set; }
        public char? Gender { get; set; }
        public DateTime? JoiningDate { get; set; }

        public clsPatientAppointmentStatusCountDTO? PatientAppointmentStatusCountDTO { get; set; }
    }
}
