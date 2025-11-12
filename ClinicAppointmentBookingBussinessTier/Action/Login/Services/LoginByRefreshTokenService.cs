using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.UserData;
using ClinicAppointmentsBookingDTO.UserDTO;
using ProjectsServices.RefreshTokenServices;

namespace ClinicAppointmentBookingBussinessTier.Action.Login.Services
{
    public class clsLoginByRefreshTokenService : clsServiceError, ILoginService<clsLoginDTO?, string>
    {

        
        public async Task<clsLoginDTO?> Login(string RefreshToken)
        {

            var HashedRefreshToen = clsRefreshTokenHelper.HashToken(RefreshToken);

            var Result = await clsUserData.LoginByRefreshToken(HashedRefreshToen);

            if (Result == null)
            {
                ErrorMessage = "Invalid or expired refresh token";
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