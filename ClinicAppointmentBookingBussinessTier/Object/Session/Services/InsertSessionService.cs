using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.SessionData;
using ClinicAppointmentsBookingDTO.SessionDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Session.Services
{
    public class clsSaveSessionService : clsServiceError, ISaveService<clsInsertSessionDTO>
    {
        public async Task<bool> Save(clsInsertSessionDTO sessionDTO)
        {
            var Result = await clsSessionData.InsertSession(sessionDTO);

            if (Result == null)
            {
                ErrorMessage = "Failed to create session - No rows affected";
                return false;
            }

            if (!string.IsNullOrEmpty(Result.Item2))
            {
                ErrorMessage = $"Failed to create session - {Result.Item2}";
                return false;
            }

            return true;
        }
    }
}