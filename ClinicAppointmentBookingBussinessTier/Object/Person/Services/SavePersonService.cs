using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.PersonData;
using ClinicAppointmentsBookingDTO.PersonDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Person.Services
{
    public class clsUpdatePersonService : clsServiceError, ISaveService<clsPersonDTO>
    {
        public async Task<bool> Save(clsPersonDTO personDTO)
        {
            var result = await clsPersonData.UpdatePerson(personDTO);

            if (result != null)
            {
                ErrorMessage = $"Failed to update person - {result}";
                return false;
            }

            return true;
        }
    }
}