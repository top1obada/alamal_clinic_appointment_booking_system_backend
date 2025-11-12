using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using Microsoft.Data.SqlClient;
using ClinicAppointmentsBookingDTO.SessionDTO;
namespace ClinicAppointmentBookingDataTier.ObjectData.SessionData
{
    public static class clsSessionData
    {
        public async static Task<Tuple<int?, string>> InsertSession(clsInsertSessionDTO sessionDto)
        {
            using (var connection = new SqlConnection(clsConnectionSettings.ConnectionString))
            using (var command = new SqlCommand("SP_InsertSession", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.AddWithValue("@UserID", sessionDto.UserID);
                command.Parameters.AddWithValue("@HashedRefreshToken", sessionDto.HashedRefreshToken);
               

                // Output parameter
                var sessionIdParam = new SqlParameter("@SessionID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(sessionIdParam);

                try
                {
                    await connection.OpenAsync();

                    int RowsAffacted = await command.ExecuteNonQueryAsync();

                    if (RowsAffacted < 1) return null;

                    return new Tuple<int?, string>((int)sessionIdParam.Value, null);
                }
                catch (Exception ex)
                {
                    return new Tuple<int?, string>(null, ex.Message);
                }
            }
          
        }


        public async static Task<Tuple< bool,string>> DropSession(string hashedRefreshCode)
        {
            using (var connection = new SqlConnection(clsConnectionSettings.ConnectionString))
            using (var command = new SqlCommand("SP_DropSession", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HashedRefreshCode", hashedRefreshCode);

                try
                {
                    await connection.OpenAsync();

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {

                        return new Tuple<bool, string>(true, null);
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    return new Tuple<bool, string>(false, ex.Message);
                }
            }
        }

    }
}
