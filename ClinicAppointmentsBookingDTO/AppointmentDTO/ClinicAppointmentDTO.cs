using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.AppointmentDTO
{
    public class clsClinicAppointmentDTO
    {

        public int ? AppointmentID { get; set; }
        public int? DoctorID { get; set; }
        public string? DoctorName { get; set; }
        public int? PatientID { get; set; }
        public string? PatientName { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public enAppointmentStatus? AppointmentStatus { get; set; }
    }
}
