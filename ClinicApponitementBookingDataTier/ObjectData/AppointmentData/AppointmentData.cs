using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using ClinicAppointmentsBookingDTO.AppointmentDTO;
using ClinicAppointmentsBookingDTO.PatientDTO;
using Microsoft.Data.SqlClient;

namespace ClinicAppointmentBookingDataTier.ObjectData.AppointmentData
{
    public static class clsAppointmentData
    {
        public static async Task<Tuple<int?, string?>?> InsertAppointment(clsAppointmentDTO appointmentDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertAppointment", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AppointmentTime", appointmentDTO.AppointmentTime);
                    command.Parameters.AddWithValue("@PatientID", appointmentDTO.PatientID);
                    command.Parameters.AddWithValue("@DoctorID", appointmentDTO.DoctorID);

                    SqlParameter appointmentIDOutput = new SqlParameter("@AppointmentID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(appointmentIDOutput);

                    try
                    {
                        await conn.OpenAsync();
                        var Result = await command.ExecuteNonQueryAsync();

                        if (Result > 0)
                        {
                            int? appointmentID = appointmentIDOutput.Value != DBNull.Value ? (int)appointmentIDOutput.Value : null;
                            return new Tuple<int?, string?>(appointmentID, null);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<int?, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<string?> ChangeAppointmentStatus(clsChangeAppointmentStatusDTO changeStatusDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_ChangeAppointmentStatus", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AppointmentID", changeStatusDTO.AppointementID);
                    command.Parameters.AddWithValue("@NewAppointmentStatus", (byte)changeStatusDTO.AppointmentStatus);

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

        public static async Task<Tuple<List<clsClinicAppointmentDTO>, string?>?> GetClinicAppointments(int pageSize, int pageNumber)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetClinicAppointments", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsClinicAppointmentDTO> appointments = new List<clsClinicAppointmentDTO>();

                            while (await reader.ReadAsync())
                            {
                                var appointment = new clsClinicAppointmentDTO
                                {
                                    AppointmentID = (int)reader["AppointmentID"],
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorName = (string)reader["DoctorName"],
                                    PatientID = (int)reader["PatientID"],
                                    PatientName = (string)reader["PatientName"],
                                    AppointmentTime = (DateTime)reader["AppointmentTime"],
                                    AppointmentStatus = (enAppointmentStatus)(byte)reader["AppointmentStatus"]
                                };

                                appointments.Add(appointment);
                            }

                            return new Tuple<List<clsClinicAppointmentDTO>, string?>(appointments, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsClinicAppointmentDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<List<clsPatientAppointmentDTO>, string?>?> GetPatientAppointments(int patientID, int pageSize, int pageNumber)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPatientAppointments", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PatientID", patientID);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsPatientAppointmentDTO> appointments = new List<clsPatientAppointmentDTO>();

                            while (await reader.ReadAsync())
                            {
                                var appointment = new clsPatientAppointmentDTO
                                {
                                    AppointmentStatus = (enAppointmentStatus)(byte)reader["AppointmentStatus"],
                                    AppointmentTime = (DateTime)reader["AppointmentTime"],
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorFullName = (string)reader["DoctorFullName"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"]
                                };

                                appointments.Add(appointment);
                            }

                            return new Tuple<List<clsPatientAppointmentDTO>, string?>(appointments, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsPatientAppointmentDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<List<clsPatientAppointmentDTO>, string?>?> GetPatientAppointmentsByAppointmentStatus(clsPatientAppointmentStatusDTO PatientAppointmentStatusDTO, int pageSize, int pageNumber)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPatientAppointmentsByAppointmentStatus", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PatientID", PatientAppointmentStatusDTO.PatientID);
                    command.Parameters.AddWithValue("@AppointmentStatus", (byte)PatientAppointmentStatusDTO.AppointmentStatus);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsPatientAppointmentDTO> appointments = new List<clsPatientAppointmentDTO>();

                            while (await reader.ReadAsync())
                            {
                                var appointment = new clsPatientAppointmentDTO
                                {
                                    AppointmentStatus = (enAppointmentStatus)(byte)reader["AppointmentStatus"],
                                    AppointmentTime = (DateTime)reader["AppointmentTime"],
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorFullName = (string)reader["DoctorFullName"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"]
                                };

                                appointments.Add(appointment);
                            }

                            return new Tuple<List<clsPatientAppointmentDTO>, string?>(appointments, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsPatientAppointmentDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<List<clsAvailableAppointmentTimeDTO>, string?>?> GetAvailableAppointmentTimes(clsAvailableTimeAppointmentsFilterDTO filterDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAvailableAppointmentDateAndTimeForDoctorByDate", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Date", filterDTO.Date);
                    command.Parameters.AddWithValue("@DoctorID", filterDTO.DoctorID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsAvailableAppointmentTimeDTO> availableTimes = new List<clsAvailableAppointmentTimeDTO>();

                            while (await reader.ReadAsync())
                            {
                                var availableTime = new clsAvailableAppointmentTimeDTO
                                {
                                    AvailableTime = reader["AvailableTime"] != DBNull.Value ? (TimeSpan)reader["AvailableTime"] : null
                                };

                                availableTimes.Add(availableTime);
                            }

                            return new Tuple<List<clsAvailableAppointmentTimeDTO>, string?>(availableTimes, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsAvailableAppointmentTimeDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<bool, string?>> IsPatientHasActiveAppointmentWithDoctor(int patientID, int doctorID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT dbo.FN_IsPatientHasActiveAppointmentWithDoctor(@PatientID, @DoctorID)", conn))
                {
                    command.Parameters.AddWithValue("@PatientID", patientID);
                    command.Parameters.AddWithValue("@DoctorID", doctorID);

                    try
                    {
                        await conn.OpenAsync();
                        var result = await command.ExecuteScalarAsync();

                        bool hasActiveAppointment = result != null && result != DBNull.Value && (bool)result;
                        return new Tuple<bool, string?>(hasActiveAppointment, null);
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<bool, string?>(false, EX.Message);
                    }
                }
            }
        }
    }
}