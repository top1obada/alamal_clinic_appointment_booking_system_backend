using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentBookingBussinessTier.Services
{
    public interface ILoginService<R,P>
    {

        public Task<R> Login(P Value);

    }
}
