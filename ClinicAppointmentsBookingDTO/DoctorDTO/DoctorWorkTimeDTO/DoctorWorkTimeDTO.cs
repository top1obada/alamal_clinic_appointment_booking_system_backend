using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.DoctorDTO.DoctorWorkTimeDTO
{
    public class clsDoctorWorkTimeDTO
    {
        public int? DoctorWorkTimeID { get; set; }
        public int? DoctorID { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
