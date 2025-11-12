using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentsBookingDTO.DoctorDTO.DoctorConsultationDTO;
using ClinicAppointmentsBookingDTO.DoctorDTO.DoctorHolidayDayDTO;
using ClinicAppointmentsBookingDTO.PersonDTO;
using ClinicAppointmentsBookingDTO.UserDTO;
using ClinicAppointmentsBookingDTO.DoctorDTO.DoctorWorkTimeDTO;
namespace ClinicAppointmentsBookingDTO.DoctorDTO
{
    public class clsCompletedDoctorDTO
    {
        public clsPersonDTO? Person { get; set; }
        public clsUserDTO? User { get; set; }
        public clsDoctorDTO? Doctor { get; set; }

        public clsDoctorConsultationDTO ?DoctorConsultation { get; set; }

        public clsDoctorWorkTimeDTO? DoctorWorkTimeDTO { get; set; } 

        public List< clsDoctorHolidayDayDTO> ? Holidays { get; set; }

    }
}
