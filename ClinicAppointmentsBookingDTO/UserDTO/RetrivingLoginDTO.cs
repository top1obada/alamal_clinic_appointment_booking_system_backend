using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.UserDTO
{
    public class ClsRetrivingLoggedInDTO
    {
        public int? PersonID { get; set; }
        public int? UserID { get; set; }
        public int? UserBranchID { get; set; }
        public string? FirstName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public enUserRole? UserRole { get; set; }
    }
}