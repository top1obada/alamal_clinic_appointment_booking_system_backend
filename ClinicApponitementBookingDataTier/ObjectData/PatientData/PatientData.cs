using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using ClinicAppointmentsBookingDTO.PatientDTO;
using Microsoft.Data.SqlClient;

namespace ClinicAppointmentBookingDataTier.ObjectData.PatientData
{
    public static class clsPatientData
    {
        public static async Task<Tuple<clsRetrivingAddPatientDTO, string?>?> InsertCompletedPatient
            (clsCompletedPatientDTO CompletedPatientDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertCompletedPatient", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Person parameters - only MiddleName gets NULL check
                    command.Parameters.AddWithValue("@FirstName", CompletedPatientDTO.Person?.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", CompletedPatientDTO.Person?.MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", CompletedPatientDTO.Person?.LastName);
                    command.Parameters.AddWithValue("@BirthDate", CompletedPatientDTO.Person?.BirthDate);
                    command.Parameters.AddWithValue("@Gender", CompletedPatientDTO.Person?.Gender);
                    command.Parameters.AddWithValue("@Nationality", CompletedPatientDTO.Person?.Nationality);

                    // User parameters - no NULL checks (required fields)
                    command.Parameters.AddWithValue("@UserName", CompletedPatientDTO.User?.UserName);
                    command.Parameters.AddWithValue("@HashedPassword", CompletedPatientDTO.User?.HashedPassword);
                    command.Parameters.AddWithValue("@Salt", CompletedPatientDTO.User?.Salt);

                    // Output parameters
                    SqlParameter personIDOutput = new SqlParameter("@PersonID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(personIDOutput);

                    SqlParameter userIDOutput = new SqlParameter("@UserID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(userIDOutput);

                    SqlParameter patientIDOutput = new SqlParameter("@PatientID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(patientIDOutput);

                    try
                    {
                        await conn.OpenAsync();
                        var Result = await command.ExecuteNonQueryAsync();

                        if (Result > 0)
                        {
                            var retrivingDTO = new clsRetrivingAddPatientDTO()
                            {
                                PersonID = personIDOutput.Value != DBNull.Value ? (int)personIDOutput.Value : null,
                                UserID = userIDOutput.Value != DBNull.Value ? (int)userIDOutput.Value : null,
                                PatientID = patientIDOutput.Value != DBNull.Value ? (int)patientIDOutput.Value : null
                            };

                            return new Tuple<clsRetrivingAddPatientDTO, string?>(retrivingDTO, null);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsRetrivingAddPatientDTO, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<clsPatientInfoDTO, string?>?> GetPatientInfo(int patientID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPatientInfo", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PatientID", patientID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var patientInfo = new clsPatientInfoDTO
                                {
                                    PersonID = (int)reader["PersonID"],
                                    PatientName = (string)reader["PatientName"],
                                    BirthDate = (DateTime)reader["BirthDate"],
                                    Gender = (char)reader["Gender"],
                                    JoiningDate = (DateTime)reader["JoiningDate"]
                                };

                                // Move to next result set for appointment status count
                                if (await reader.NextResultAsync())
                                {
                                    if (await reader.ReadAsync())
                                    {
                                        patientInfo.PatientAppointmentStatusCountDTO = new clsPatientAppointmentStatusCountDTO
                                        {
                                            PendingAppointments = (int)reader["Pending Appointments"],
                                            ConfirmedAppointments = (int)reader["Confirmed Appointments"],
                                            RejectedAppointments = (int)reader["Rejected Appointments"],
                                            CancelledAppointments = (int)reader["Cancelled Appointments"],
                                            CompletedAppointments = (int)reader["Copleted Appointments"]
                                        };
                                    }
                                }

                                return new Tuple<clsPatientInfoDTO, string?>(patientInfo, null);
                            }
                            else
                            {
                                return new Tuple<clsPatientInfoDTO, string?>(null, "Patient not found");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsPatientInfoDTO, string?>(null, EX.Message);
                    }
                }
            }
        }


        public static async Task<Tuple<List<clsClinicPatientDTO>, string?>?> GetPatients(int pageSize, int pageNumber)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPatients", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsClinicPatientDTO> patients = new List<clsClinicPatientDTO>();

                            while (await reader.ReadAsync())
                            {
                                var patient = new clsClinicPatientDTO
                                {
                                    PatientID = (int)reader["PatientID"],
                                    PatientName = (string)reader["PatientName"],
                                    Gender = Convert.ToChar( reader["Gender"]),
                                    Nationality = (string)reader["Nationality"],
                                    CompletedAppointmentsCount = (int)reader["CompletedAppointmentsCount"],
                                    JoiningDate = (DateTime)reader["JoiningDate"]
                                };

                                patients.Add(patient);
                            }

                            return new Tuple<List<clsClinicPatientDTO>, string?>(patients, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsClinicPatientDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }


        public static async Task<Tuple<clsPatientDetailsDTO, string?>?> GetPatientDetails(int patientID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPatientDetails", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PatientID", patientID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var patientDetails = new clsPatientDetailsDTO();

                         
                            if (await reader.ReadAsync())
                            {
                                patientDetails.JoiningDate = (DateTime)reader["JoiningDate"];
                            }
                            else
                            {
                                return new Tuple<clsPatientDetailsDTO, string?>(null, "Patient not found");
                            }

                            
                            if (await reader.NextResultAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    patientDetails.AppointmentStatusCount = new clsPatientAppointmentStatusCountDTO
                                    {
                                        PendingAppointments = (int)reader["Pending Appointments"],
                                        ConfirmedAppointments = (int)reader["Confirmed Appointments"],
                                        RejectedAppointments = (int)reader["Rejected Appointments"],
                                        CancelledAppointments = (int)reader["Cancelled Appointments"],
                                        CompletedAppointments = (int)reader["Copleted Appointments"]
                                    };
                                }
                            }

                            return new Tuple<clsPatientDetailsDTO, string?>(patientDetails, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsPatientDetailsDTO, string?>(null, EX.Message);
                    }
                }
            }
        }

    }
}