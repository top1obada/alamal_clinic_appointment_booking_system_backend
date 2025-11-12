using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.AppointmentDTO
{
    public enum enAppointmentStatus
    {
        Pending = 1,
        Confirmed = 2,
        Rejected = 3,
        Cancelled = 4,
        Completed = 5
    }

    public class clsAppointmentDTO
    {
        public int? AppointmentID { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public int? PatientID { get; set; }
        public int? DoctorID { get; set; }
        public enAppointmentStatus? AppointmentStatus { get; set; }
    }
}