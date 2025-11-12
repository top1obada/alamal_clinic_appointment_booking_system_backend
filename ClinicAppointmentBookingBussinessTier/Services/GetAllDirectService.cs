using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentBookingBussinessTier.Services
{
    public interface IGetAllDirectService<R>
    {

        public Task<R> Get(int PageSize, int PageNumber);

    }
}
