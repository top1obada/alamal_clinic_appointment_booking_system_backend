using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.PatientData;
using ClinicAppointmentsBookingDTO.PatientDTO;
using ClinicAppointmentsBookingDTO.UserDTO;
using ProjectsServices.PasswordServices;

namespace ClinicAppointmentBookingBussinessTier.Object.Patient.Services.SaveCompletedPatient
{
    public class clsSaveCompletedPatientService : clsServiceError, ISaveService<CompletedPatientWithLoginRequirements>
    {
        public async Task<bool> Save(CompletedPatientWithLoginRequirements CompletedPatientDTO)
        {
            var HashedResult = clsPasswordEncrypt.HashPassword(CompletedPatientDTO.LoginRequirmentsDTO.Password);

            var CompletedPatient = CompletedPatientDTO.CompletedPatientDTO;

            CompletedPatient.User.HashedPassword = HashedResult.Hash;
            CompletedPatient.User.Salt = HashedResult.Salt;

            var Result = await clsPatientData.InsertCompletedPatient(CompletedPatient);

            if (Result == null)
            {
                ErrorMessage = "Failed To Add Patient";
                return false;
            }

            if (Result.Item2 != null)
            {
                ErrorMessage = $"Failed To Add Patient - {Result.Item2}";
                return false;
            }

            CompletedPatient.User.JoiningDate = DateTime.Now;
            CompletedPatient.User.UserID = Result.Item1.UserID;
            CompletedPatient.Person.PersonID = Result.Item1.PersonID;
            CompletedPatient.Patient.PatientID = Result.Item1.PatientID;

            return true;
        }
    }
}