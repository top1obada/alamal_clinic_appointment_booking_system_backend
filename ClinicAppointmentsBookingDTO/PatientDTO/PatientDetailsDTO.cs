using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.PatientDTO
{
    public class clsPatientDetailsDTO
    {
        public DateTime? JoiningDate { get; set; }
        public clsPatientAppointmentStatusCountDTO? AppointmentStatusCount { get; set; }
    }
}