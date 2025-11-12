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
    public class clsGetClinicDoctorsBySpecialNameService : clsServiceError, IGetAllService<List<clsClinicDoctorDTO>, string>
    {
        public async Task<List<clsClinicDoctorDTO>> Get(int PageNumber, int PageSize, string? Value)
        {
            var Result = await clsDoctorData.GetClinicDoctorsBySpecialName(Value, PageSize, PageNumber);

            if (Result == null)
            {
                ErrorMessage = "Failed to retrieve doctors by specialty";
                return new List<clsClinicDoctorDTO>();
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to retrieve doctors by specialty - {Result.Item2}";
                return new List<clsClinicDoctorDTO>();
            }

            return Result.Item1 ?? new List<clsClinicDoctorDTO>();
        }
    }
}