using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.AppointmentDTO
{
    public class clsChangeAppointmentStatusDTO
    {

        public int ? AppointementID { get; set; }

        public enAppointmentStatus? AppointmentStatus { get; set; }

    }
}
