using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.DataTypes;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.DoctorData;
using ClinicAppointmentsBookingDTO.DoctorDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Doctor.Services.UpdateCompletedDoctor
{
    public class clsUpdateCompletedDoctorService : clsServiceError, IUpdateService<bool, clsCompletedDoctorDTO>
    {
        public async Task<bool> Update(clsCompletedDoctorDTO CompletedDoctorDTO)
        {
            if (CompletedDoctorDTO?.Doctor?.DoctorID == null)
            {
                ErrorMessage = "Doctor ID is required";
                return false;
            }

            var Result = await clsDoctorData.UpdateCompletedDoctor(CompletedDoctorDTO);

            if (Result == null)
            {
                ErrorMessage = "Failed to update doctor";
                return false;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed to update doctor - {Result.Item2}";
                return false;
            }

            return Result.Item1;
        }
    }
}