using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsServices.JWTServices
{
    public class clsJWTTemplate
    {
        public string? Issuer { get; set; } = null;

        public string? Audience { get; set; } = null;

        public int? DurationInMinutes { get; set; } = null;

        public string? Key { get; set; } = null;


    }
}
