using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.DoctorDTO.DoctorConsultationDTO
{
    public class clsDoctorConsultationDTO
    {
        public int? DoctorConsultationID { get; set; }
        public int? DoctorID { get; set; }
        public float? ConsultationFee { get; set; }
        public short? ConsultationDurationInMinutes { get; set; }
    }
}
