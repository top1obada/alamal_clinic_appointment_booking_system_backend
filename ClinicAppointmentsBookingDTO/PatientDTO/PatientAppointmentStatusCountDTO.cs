using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.PatientDTO
{
    public class clsPatientAppointmentStatusCountDTO
    {
        public int? PendingAppointments { get; set; }
        public int? ConfirmedAppointments { get; set; }
        public int? RejectedAppointments { get; set; }
        public int? CancelledAppointments { get; set; }
        public int? CompletedAppointments { get; set; }
    }
}
