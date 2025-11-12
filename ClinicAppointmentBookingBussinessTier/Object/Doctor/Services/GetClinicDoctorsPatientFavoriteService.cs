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
    public class clsGetClinicDoctorsPatientFavoriteService : clsServiceError, IGetAllService<List<clsClinicDoctorDTO>, int>
    {
        public async Task<List<clsClinicDoctorDTO>> Get(int pageNumber, int pageSize, int patientID)
        {
            var Result = await clsDoctorData.GetClinicDoctorsPatientFavorite(pageSize, pageNumber, patientID);

            if (Result == null)
            {
                ErrorMessage = "Failed to get patient favorite doctors";
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