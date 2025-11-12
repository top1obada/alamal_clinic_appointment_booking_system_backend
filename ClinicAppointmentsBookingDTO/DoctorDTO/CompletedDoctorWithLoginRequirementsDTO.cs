using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentsBookingDTO.UserDTO;

namespace ClinicAppointmentsBookingDTO.DoctorDTO
{
    public class clsCompletedDoctorWithLoginRequirementsDTO
    {

        public clsCompletedDoctorDTO ? CompletedDoctorDTO { get; set; }

        public clsLoginRequirementsDTO? LoginRequirementsDTO { get; set; }

    }
}
