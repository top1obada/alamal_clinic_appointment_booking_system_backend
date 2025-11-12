using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentBookingBussinessTier.Services
{
    public interface ISaveService<P>
    {

        public Task<bool> Save(P Value);

    }
}
