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
    public class clsSaveAppointmentService : clsServiceError, ISaveService<clsAppointmentDTO>
    {
        public async Task<bool> Save(clsAppointmentDTO appointmentDTO)
        {

          
            

            var Result = await clsAppointmentData.InsertAppointment(appointmentDTO);

            if (Result == null)
            {
                ErrorMessage = "Failed to create appointment";
                return false;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to create appointment - {Result.Item2}";
                return false;
            }

            appointmentDTO.AppointmentID = Result.Item1;
            return true;
        }
    }
}