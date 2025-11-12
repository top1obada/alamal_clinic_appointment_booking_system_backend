using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentsBookingDTO.PatientDTO;
using ClinicAppointmentsBookingDTO.UserDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Patient.Services.SaveCompletedPatient
{
    public class CompletedPatientWithLoginRequirements
    {
        public clsCompletedPatientDTO? CompletedPatientDTO { get; set; }
        public clsLoginRequirementsDTO? LoginRequirmentsDTO { get; set; }
    }
}
