using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.AppointmentData;
using ClinicAppointmentsBookingDTO.AppointmentDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Appointment.Services
{
    public class clsGetAvailableAppointmentTimesService : clsServiceError, IGetConditionalAllService<List<clsAvailableAppointmentTimeDTO>, clsAvailableTimeAppointmentsFilterDTO>
    {
        public async Task<List<clsAvailableAppointmentTimeDTO>> Get(clsAvailableTimeAppointmentsFilterDTO filterDTO)
        {
            var Result = await clsAppointmentData.GetAvailableAppointmentTimes(filterDTO);

            if (Result == null)
            {
                ErrorMessage = "Failed to get available appointment times";
                return null;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = Result.Item2;
                return null;
            }

            return Result.Item1;
        }
    }
}