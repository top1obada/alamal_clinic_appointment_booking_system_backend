using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.DataTypes;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.DoctorData;
using ClinicAppointmentsBookingDTO.DoctorDTO;
using ClinicAppointmentsBookingDTO.UserDTO;
using ProjectsServices.PasswordServices;

namespace ClinicAppointmentBookingBussinessTier.Object.Doctor.Services.SaveCompletedDoctor
{

    

    public class clsSaveCompletedDoctorService : clsServiceError, ISaveService<CompletedDoctorWithLoginRequirements>
    {
       


        public async Task<bool> Save(CompletedDoctorWithLoginRequirements CompletedDoctorDTO)
        {

            var HashedResult = clsPasswordEncrypt.HashPassword(CompletedDoctorDTO.LoginRequirmentsDTO.Password);

            var CompletedDoctor = CompletedDoctorDTO.CompletedDoctorDTO;

            CompletedDoctor.User.HashedPassword = HashedResult.Hash;

            CompletedDoctor.User.Salt = HashedResult.Salt;

            CompletedDoctor.User.UserName = CompletedDoctorDTO.LoginRequirmentsDTO.UserName;

            var Result = await clsDoctorData.InsertCompletedDoctor(CompletedDoctor);

            if(Result == null)
            {
                ErrorMessage = "Failed To Add Doctor";
                return false;
            }

            if(Result.Item2 != null)
            {

                ErrorMessage = $"Failed To Add Doctor - {Result.Item2}";
                return false;

            }

            CompletedDoctor.User.JoiningDate = DateTime.Now;

            CompletedDoctor.User.UserID = Result.Item1.UserID;

            CompletedDoctor.Person.PersonID = Result.Item1.PersonID;

            CompletedDoctor.Doctor.DoctorID = Result.Item1.DoctorID;

            CompletedDoctor.DoctorConsultation.DoctorConsultationID = Result.Item1.DoctorConsultationID;

            return true;



        }

    }
}
