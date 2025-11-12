using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointmentsBookingDTO.UserDTO
{

    public enum enUserRole { eReceptionist = 1,eDoctor = 2,ePatient = 3}

    public class clsUserDTO
    {
        public int? UserID { get; set; }
        public int? PersonID { get; set; }
        public string? UserName { get; set; }
        public byte[]? HashedPassword { get; set; }
        public byte[]? Salt { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
