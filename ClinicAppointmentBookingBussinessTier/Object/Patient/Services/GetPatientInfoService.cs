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
    public class clsGetPatientInfoService : clsServiceError, IGetService<clsPatientInfoDTO, int>
    {
        public async Task<clsPatientInfoDTO> Get(int PatientID)
        {
            var Result = await clsPatientData.GetPatientInfo(PatientID);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve patient info";
                return null;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve patient info - {Result.Item2}";
                return null;
            }

            return Result.Item1;
        }
    }
}