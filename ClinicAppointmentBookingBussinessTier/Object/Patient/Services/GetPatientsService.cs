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
    public class clsGetPatientsService : clsServiceError, IGetAllDirectService<List<clsClinicPatientDTO>>
    {
        public async Task<List<clsClinicPatientDTO>> Get(int PageSize, int PageNumber)
        {
            var Result = await clsPatientData.GetPatients(PageSize, PageNumber);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve patients";
                return new List<clsClinicPatientDTO>();
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve patients - {Result.Item2}";
                return new List<clsClinicPatientDTO>();
            }

            return Result.Item1 ?? new List<clsClinicPatientDTO>();
        }
    }
}