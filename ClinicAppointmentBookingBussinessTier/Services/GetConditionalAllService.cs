using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentBookingBussinessTier.Services
{
    public interface IGetConditionalAllService<R,P>
    {

        public Task<R> Get(P Value);

    }
}
