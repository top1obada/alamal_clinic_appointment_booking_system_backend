using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.DataTypes;
using ClinicAppointmentBookingDataTier.ObjectData.PersonData;
using ClinicAppointmentsBookingDTO.PersonDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Person
{
    public class clsPerson
    {
        public int? PersonID { get; internal set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public char? Gender { get; set; }
        public string? Nationality { get; set; }

        public clsModes.enSaveMode SaveMode { get; internal set; }

        public clsPersonDTO PersonDTO
        {
            get
            {
                return new clsPersonDTO()
                {
                    PersonID = this.PersonID,
                    FirstName = this.FirstName,
                    MiddleName = this.MiddleName,
                    LastName = this.LastName,
                    BirthDate = this.BirthDate,
                    Gender = this.Gender,
                    Nationality = this.Nationality
                };
            }

            set
            {
                
                this.FirstName = value.FirstName;
                this.MiddleName = value.MiddleName;
                this.LastName = value.LastName;
                this.BirthDate = value.BirthDate;
                this.Gender = value.Gender;
                this.Nationality = value.Nationality;
            }
        }

        public clsPerson()
        {
            SaveMode = clsModes.enSaveMode.eAdd;
        }

        private clsPerson(clsPersonDTO personDTO)
        {
            this.PersonID = personDTO.PersonID;
            SaveMode = clsModes.enSaveMode.eUpdate;
            this.PersonDTO = personDTO;
        }

        public static async Task<clsPerson> Find(int personID)
        {
            var Result = await clsPersonData.GetPersonInfo(personID);

            if (Result == null) return null;

            if (Result.Item1 == null) return null;

            return new clsPerson(Result.Item1);
        }
    }
}