using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentBookingBussinessTier.Services
{
    public interface IGetAllService<R,P>
    {

        public Task<R> Get(int PageNumber, int PageSize, P Value); 

    }
}
