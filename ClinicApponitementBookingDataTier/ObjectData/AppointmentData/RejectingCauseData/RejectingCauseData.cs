using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using ClinicAppointmentsBookingDTO.AppointmentDTO.RejectingCauseDTO;
using Microsoft.Data.SqlClient;

namespace ClinicAppointmentBookingDataTier.ObjectData.RejectingCauseData
{
    public static class clsRejectingCauseData
    {
        public static async Task<string?> InsertRejectingCause(clsRejectingCauseDTO rejectingCauseDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertRejectingCause", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AppointmentID", rejectingCauseDTO.AppointmentID);
                    command.Parameters.AddWithValue("@Text", rejectingCauseDTO.Text);

                    try
                    {
                        await conn.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return null;
                    }
                    catch (Exception EX)
                    {
                        return EX.Message;
                    }
                }
            }
        }

        public static async Task<string?> UpdateRejectingCause(clsRejectingCauseDTO rejectingCauseDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateRejectingCause", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AppointmentID", rejectingCauseDTO.AppointmentID);
                    command.Parameters.AddWithValue("@Text", rejectingCauseDTO.Text);

                    try
                    {
                        await conn.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return null;
                    }
                    catch (Exception EX)
                    {
                        return EX.Message;
                    }
                }
            }
        }

        public static async Task<Tuple<clsRejectingCauseDTO, string?>?> GetRejectingCauseByAppointment(int appointmentID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetRejectingCauseByAppointment", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AppointmentID", appointmentID);

                    try
                    {
                        await conn.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var rejectingCause = new clsRejectingCauseDTO
                                {
                                    AppointmentID = (int)reader["AppointmentID"],
                                    Text = (string)reader["Text"],
                                    RejectedDate = (DateTime)reader["RejectedDate"]
                                };

                                return new Tuple<clsRejectingCauseDTO, string?>(rejectingCause, null);
                            }
                            else
                            {
                                return new Tuple<clsRejectingCauseDTO, string?>(null, "Rejecting cause not found");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsRejectingCauseDTO, string?>(null, EX.Message);
                    }
                }
            }
        }
    }
}