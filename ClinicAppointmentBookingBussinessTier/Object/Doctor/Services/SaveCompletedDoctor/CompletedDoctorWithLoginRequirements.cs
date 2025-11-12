using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentsBookingDTO.DoctorDTO;
using ClinicAppointmentsBookingDTO.UserDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Doctor.Services.SaveCompletedDoctor
{
    public class CompletedDoctorWithLoginRequirements
    {

        public clsCompletedDoctorDTO? CompletedDoctorDTO { get; set; }

        public clsLoginRequirementsDTO? LoginRequirmentsDTO { get; set; }

    }
}
