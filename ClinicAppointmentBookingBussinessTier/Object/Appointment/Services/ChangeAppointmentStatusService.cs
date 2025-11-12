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
    public class clsChangeAppointmentStatusService : clsServiceError, ISaveService<clsChangeAppointmentStatusDTO>
    {
        public async Task<bool> Save(clsChangeAppointmentStatusDTO changeStatusDTO)
        {
            if (changeStatusDTO.AppointmentStatus.Value == enAppointmentStatus.Pending)
            {
                ErrorMessage = "Appointments cannot be reverted to pending status once they have been processed. " +
                              "Please choose a different status: Confirmed, Completed, Cancelled, or Rejected.";
                return false;
            }

            var error = await clsAppointmentData.ChangeAppointmentStatus(changeStatusDTO);

            if (error != null)
            {
                ErrorMessage = error;
                return false;
            }

            return true;
        }
    }
}