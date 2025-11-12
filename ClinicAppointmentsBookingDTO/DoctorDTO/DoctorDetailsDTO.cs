using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.DoctorDTO
{
    public class clsDoctorDetailsDTO
    {
        public string? DoctorName { get; set; }
        public char? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? SpecialtiesName { get; set; }
        public int? ConsultationDurationInMinutes { get; set; }
        public float? ConsultationFee { get; set; }
        public string? SpecialtiesDescription { get; set; }
        public string? DoctorImageLink { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public List<string>? Holidays { get; set; }

    }
}
