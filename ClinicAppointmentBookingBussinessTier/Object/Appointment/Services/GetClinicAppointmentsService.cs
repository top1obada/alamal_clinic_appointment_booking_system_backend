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
    public class clsGetClinicAppointmentsService : clsServiceError, IGetAllDirectService<List<clsClinicAppointmentDTO>>
    {
        public async Task<List<clsClinicAppointmentDTO>> Get(int PageSize, int PageNumber)
        {
            var Result = await clsAppointmentData.GetClinicAppointments(PageSize, PageNumber);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve clinic appointments";
                return new List<clsClinicAppointmentDTO>();
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve clinic appointments - {Result.Item2}";
                return new List<clsClinicAppointmentDTO>();
            }

            return Result.Item1 ?? new List<clsClinicAppointmentDTO>();
        }
    }
}