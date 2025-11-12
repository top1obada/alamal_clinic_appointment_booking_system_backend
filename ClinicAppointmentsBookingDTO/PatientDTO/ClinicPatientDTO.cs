using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.PatientDTO
{
    public class clsClinicPatientDTO
    {
        public int? PatientID { get; set; }
        public string? PatientName { get; set; }
        public char? Gender { get; set; }
        public string? Nationality { get; set; }
        public int? CompletedAppointmentsCount { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
