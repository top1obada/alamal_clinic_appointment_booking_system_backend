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
    public class clsGetClinicDoctorsService : clsServiceError, IGetAllDirectService<List<clsClinicDoctorDTO>>
    {
        public async Task<List<clsClinicDoctorDTO>> Get(int PageSize, int PageNumber)
        {
            var Result = await clsDoctorData.GetClinicDoctors(PageSize, PageNumber);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve doctors";
                return new List<clsClinicDoctorDTO>();
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve doctors - {Result.Item2}";
                return new List<clsClinicDoctorDTO>();
            }

            return Result.Item1 ?? new List<clsClinicDoctorDTO>();
        }
    }
}