using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.DoctorDTO
{
    public class clsClinicDoctorDTO
    {
        public int? DoctorID { get; set; }
        public string? DoctorName { get; set; }
        public string? SpecialtiesName { get; set; }
        public string? DoctorImageLink { get; set; }
        public char? Gender { get; set; }
    }
}