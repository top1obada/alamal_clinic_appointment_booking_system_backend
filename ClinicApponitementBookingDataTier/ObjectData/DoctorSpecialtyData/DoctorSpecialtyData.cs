using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicAppointmentBookingDataTier.ObjectData.DoctorData
{
    public static class clsDoctorSpecialtiesData
    {
        public static async Task<Tuple<List<string>, string?>> GetDoctorSpecialties()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetDoctorSpecialties", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        await conn.OpenAsync();
                        var specialties = new List<string>();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                specialties.Add((string)reader["SpecialtiesName"]);
                            }
                        }

                        return new Tuple<List<string>, string?>(specialties, null);
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<string>, string?>(null, EX.Message);
                    }
                }
            }
        }
    }
}