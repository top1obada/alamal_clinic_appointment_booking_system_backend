using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.AppointmentData;
using ClinicAppointmentBookingDataTier.ObjectData.PatientData;
using ClinicAppointmentsBookingDTO.AppointmentDTO;
using ClinicAppointmentsBookingDTO.PatientDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Patient.Services
{
    

    public class clsGetPatientAppointmentsByAppointmentStatusService : clsServiceError, IGetAllService<List<clsPatientAppointmentDTO>, clsPatientAppointmentStatusDTO>
    {
        public async Task<List<clsPatientAppointmentDTO>> Get(int PageNumber, int PageSize, clsPatientAppointmentStatusDTO Value)
        {
            var Result = await clsAppointmentData.GetPatientAppointmentsByAppointmentStatus(Value, PageSize, PageNumber);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve patient appointments by status";
                return new List<clsPatientAppointmentDTO>();
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve patient appointments by status - {Result.Item2}";
                return new List<clsPatientAppointmentDTO>();
            }

            return Result.Item1 ?? new List<clsPatientAppointmentDTO>();
        }
    }
}