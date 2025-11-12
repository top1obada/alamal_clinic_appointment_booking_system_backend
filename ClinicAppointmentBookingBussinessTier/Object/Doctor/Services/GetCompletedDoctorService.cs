using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.DataTypes;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.DoctorData;
using ClinicAppointmentsBookingDTO.DoctorDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Doctor.Services.FindDoctor
{
    public class clsFindDoctorService : clsServiceError, IGetService<clsCompletedDoctorDTO, int>
    {
        public async Task<clsCompletedDoctorDTO> Get(int DoctorID)
        {
            if (DoctorID <= 0)
            {
                ErrorMessage = "Invalid Doctor ID";
                return null;
            }

            var Result = await clsDoctorData.FindDoctorByID(DoctorID);

            if (Result == null)
            {
                ErrorMessage = "Failed to find doctor";
                return null;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to find doctor - {Result.Item2}";
                return null;
            }

            return Result.Item1;
        }
    }
}