using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.PatientData;
using ClinicAppointmentsBookingDTO.PatientDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Patient.Services
{
    public class clsGetPatientDetailsService : clsServiceError, IGetService<clsPatientDetailsDTO, int>
    {
        public async Task<clsPatientDetailsDTO> Get(int Value)
        {
            var Result = await clsPatientData.GetPatientDetails(Value);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve patient details";
                return null;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve patient details - {Result.Item2}";
                return null;
            }

            return Result.Item1;
        }
    }
}