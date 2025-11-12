using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.DoctorData;
using ClinicAppointmentsBookingDTO.DoctorDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Doctor.Services
{
    public class clsGetDoctorDetailsService : clsServiceError, IGetService<clsDoctorDetailsDTO, int>
    {
        public async Task<clsDoctorDetailsDTO> Get(int DoctorID)
        {
            var Result = await clsDoctorData.GetDoctorDetails(DoctorID);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve doctor details";
                return null;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve doctor details - {Result.Item2}";
                return null;
            }

            return Result.Item1;
        }
    }
}