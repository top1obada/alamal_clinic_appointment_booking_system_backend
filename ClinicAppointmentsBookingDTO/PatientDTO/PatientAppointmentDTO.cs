using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentsBookingDTO.AppointmentDTO;

namespace ClinicAppointmentsBookingDTO.PatientDTO
{
    public class clsPatientAppointmentDTO
    {
        public enAppointmentStatus? AppointmentStatus { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public int? DoctorID { get; set; }
        public string? DoctorFullName { get; set; }
        public string? SpecialtiesName { get; set; }
    }

   
}
