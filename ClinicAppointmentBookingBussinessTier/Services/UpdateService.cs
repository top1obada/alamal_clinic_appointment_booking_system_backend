using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentBookingBussinessTier.Services
{
    public interface IUpdateService<R,P>
    {

        Task<R> Update(P Value);

    }
}
