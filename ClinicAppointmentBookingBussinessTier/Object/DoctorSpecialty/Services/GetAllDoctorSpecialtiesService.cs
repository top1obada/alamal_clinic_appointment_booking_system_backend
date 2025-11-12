using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.DoctorData;

namespace ClinicAppointmentBookingBussinessTier.Object.Doctor.Services
{
    public class clsGetDoctorSpecialtiesService : clsServiceError, IGetAllRecordsService<List<string>>
    {
        public async Task<List<string>> Get()
        {
            var Result = await clsDoctorSpecialtiesData.GetDoctorSpecialties();

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve doctor specialties";
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