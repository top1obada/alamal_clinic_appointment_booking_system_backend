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
    public class clsGetClinicDoctorsByNameService : clsServiceError, IGetAllService<List<clsClinicDoctorDTO>, string>
    {
        public async Task<List<clsClinicDoctorDTO>> Get(int pageNumber, int pageSize, string doctorName)
        {
            var Result = await clsDoctorData.GetClinicDoctorsByName(doctorName, pageSize, pageNumber);

            if (Result == null)
            {
                ErrorMessage = "Failed to search doctors by name";
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