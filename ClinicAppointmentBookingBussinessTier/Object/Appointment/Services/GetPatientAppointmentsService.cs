using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.AppointmentData;
using ClinicAppointmentBookingDataTier.ObjectData.PatientData;
using ClinicAppointmentsBookingDTO.PatientDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Patient.Services
{
    public class clsGetPatientAppointmentsService : clsServiceError, IGetAllService<List<clsPatientAppointmentDTO>, int>
    {
        public async Task<List<clsPatientAppointmentDTO>> Get(int PageNumber, int PageSize, int Value)
        {
            var Result = await clsAppointmentData.GetPatientAppointments(Value, PageSize, PageNumber);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve patient appointments";
                return new List<clsPatientAppointmentDTO>();
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve patient appointments - {Result.Item2}";
                return new List<clsPatientAppointmentDTO>();
            }

            return Result.Item1 ?? new List<clsPatientAppointmentDTO>();
        }
    }
}