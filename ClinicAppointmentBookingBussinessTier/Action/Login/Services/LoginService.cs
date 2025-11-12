using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.UserData;
using ClinicAppointmentsBookingDTO.UserDTO;
using ProjectsServices.PasswordServices;
namespace ClinicAppointmentBookingBussinessTier.Action.Login.Services
{
    public class clsLoginService : clsServiceError, ILoginService<clsLoginDTO?,clsLoginRequirementsDTO>
    {     
        public async Task<clsLoginDTO?> Login(clsLoginRequirementsDTO LoginRequirementsDTO)
        {

            var Result = await clsUserData.Login(LoginRequirementsDTO.UserName);

            if (Result == null)
            {
                ErrorMessage = "UserName/Password Not Correct";
                return null;
            }

            if(Result.Item2 != null)
            {
                ErrorMessage = Result.Item2;
            }

            var PasswordResult = clsPasswordEncrypt.VerifyPassword(LoginRequirementsDTO.Password,
                Result.Item1.HashedPassword, Result.Item1.Salt);

            if (PasswordResult)
            {
                return Result.Item1;
            }
            else
            {
                ErrorMessage = "UserName/Password Not Correct";
                return null;
            }

        }

    }
}
