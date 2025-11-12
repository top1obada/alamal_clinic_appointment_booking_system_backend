using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.AppointmentDTO.RejectingCauseDTO
{
    public class clsRejectingCauseDTO
    {
        public int? AppointmentID { get; set; }
        public string? Text { get; set; }
        public DateTime? RejectedDate { get; set; }
    }
}
