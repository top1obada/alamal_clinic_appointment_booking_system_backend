using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using ClinicAppointmentsBookingDTO.DoctorDTO;
using ClinicAppointmentsBookingDTO.DoctorDTO.DoctorConsultationDTO;
using ClinicAppointmentsBookingDTO.DoctorDTO.DoctorHolidayDayDTO;
using ClinicAppointmentsBookingDTO.DoctorDTO.DoctorWorkTimeDTO;
using ClinicAppointmentsBookingDTO.PersonDTO;
using Microsoft.Data.SqlClient;

namespace ClinicAppointmentBookingDataTier.ObjectData.DoctorData
{
    public static class clsDoctorData
    {
        public static async Task<Tuple<clsRetrivingAddDoctorDTO, string?>?> InsertCompletedDoctor
       (clsCompletedDoctorDTO CompletedDoctorDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertCompletedDoctor", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FirstName", CompletedDoctorDTO.Person?.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", CompletedDoctorDTO.Person?.MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", CompletedDoctorDTO.Person?.LastName);
                    command.Parameters.AddWithValue("@BirthDate", CompletedDoctorDTO.Person?.BirthDate);
                    command.Parameters.AddWithValue("@Gender", CompletedDoctorDTO.Person?.Gender);
                    command.Parameters.AddWithValue("@Nationality", CompletedDoctorDTO.Person?.Nationality);

                    command.Parameters.AddWithValue("@UserName", CompletedDoctorDTO.User?.UserName);
                    command.Parameters.AddWithValue("@HashedPassword", CompletedDoctorDTO.User?.HashedPassword);
                    command.Parameters.AddWithValue("@Salt", CompletedDoctorDTO.User?.Salt);

                    command.Parameters.AddWithValue("@SpecialtiesName", CompletedDoctorDTO.Doctor?.SpecialtiesName);

                    command.Parameters.AddWithValue("@ConsultationFee", CompletedDoctorDTO.DoctorConsultation?.ConsultationFee);
                    command.Parameters.AddWithValue("@ConsultationDurationInMinutes", CompletedDoctorDTO.DoctorConsultation?.ConsultationDurationInMinutes);

                    string holidayDays = "";
                    if (CompletedDoctorDTO.Holidays != null && CompletedDoctorDTO.Holidays.Any())
                    {
                        holidayDays = string.Join(",", CompletedDoctorDTO.Holidays.Select(h => h.Day));
                    }
                    command.Parameters.AddWithValue("@HolidayDays", holidayDays);

                    command.Parameters.AddWithValue("@StartTime", CompletedDoctorDTO.DoctorWorkTimeDTO?.StartTime);
                    command.Parameters.AddWithValue("@EndTime", CompletedDoctorDTO.DoctorWorkTimeDTO?.EndTime);

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

                    SqlParameter doctorIDOutput = new SqlParameter("@DoctorID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(doctorIDOutput);

                    SqlParameter doctorConsultationIDOutput = new SqlParameter("@DoctorConsultationID", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(doctorConsultationIDOutput);

                    try
                    {
                        await conn.OpenAsync();
                        var Result = await command.ExecuteNonQueryAsync();

                        if (Result > 0)
                        {
                            var retrivingDTO = new clsRetrivingAddDoctorDTO()
                            {
                                PersonID = personIDOutput.Value != DBNull.Value ? (int)personIDOutput.Value : null,
                                UserID = userIDOutput.Value != DBNull.Value ? (int)userIDOutput.Value : null,
                                DoctorID = doctorIDOutput.Value != DBNull.Value ? (int)doctorIDOutput.Value : null,
                                DoctorConsultationID = doctorConsultationIDOutput.Value != DBNull.Value ? (int)doctorConsultationIDOutput.Value : null
                            };

                            return new Tuple<clsRetrivingAddDoctorDTO, string?>(retrivingDTO, null);
                        }
                        else
                        {
                            return null;
                        }

                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsRetrivingAddDoctorDTO, string?>(null, EX.Message);
                    }
                }
            }
        }
        public static async Task<Tuple<List<clsClinicDoctorDTO>, string?>?> GetClinicDoctors(int pageSize, int pageNumber)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetClinicDoctors", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsClinicDoctorDTO> doctors = new List<clsClinicDoctorDTO>();

                            while (await reader.ReadAsync())
                            {
                                var doctor = new clsClinicDoctorDTO
                                {
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorName = (string)reader["DoctorName"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"],
                                    DoctorImageLink = reader["DoctorImageLink"] != DBNull.Value ? (string)reader["DoctorImageLink"] : null,
                                    Gender = Convert.ToChar( reader["Gender"])
                                };

                                doctors.Add(doctor);
                            }

                            return new Tuple<List<clsClinicDoctorDTO>, string?>(doctors, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsClinicDoctorDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<List<clsClinicDoctorDTO>, string?>?> GetClinicDoctorsBySpecialName(string? specialName, int pageSize, int pageNumber)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetClinicDoctorsBySpecialName", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SpecialName", specialName == null ? (object)DBNull.Value : specialName);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsClinicDoctorDTO> doctors = new List<clsClinicDoctorDTO>();

                            while (await reader.ReadAsync())
                            {
                                var doctor = new clsClinicDoctorDTO
                                {
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorName = (string)reader["DoctorName"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"],
                                    DoctorImageLink = reader["DoctorImageLink"] != DBNull.Value ? (string)reader["DoctorImageLink"] : null,
                                    Gender = Convert.ToChar(reader["Gender"])
                                };

                                doctors.Add(doctor);
                            }

                            return new Tuple<List<clsClinicDoctorDTO>, string?>(doctors, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsClinicDoctorDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<clsDoctorDetailsDTO, string?>?> GetDoctorDetails(int doctorID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetDoctorDetails", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DoctorID", doctorID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var doctorDetails = new clsDoctorDetailsDTO
                                {
                                    DoctorName = (string)reader["DoctorName"],
                                    Gender = Convert.ToChar( reader["Gender"]),
                                    BirthDate = (DateTime)reader["BirthDate"],
                                    Nationality = (string)reader["Nationality"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"],
                                    ConsultationDurationInMinutes = (short)reader["ConsultationDurationInMinutes"],
                                    ConsultationFee =Convert.ToSingle( reader["ConsultationFee"]),
                                    SpecialtiesDescription = (string)reader["SpecialtiesDescription"],
                                    DoctorImageLink = reader["DoctorImageLink"] != DBNull.Value ? (string)reader["DoctorImageLink"] : null,
                                    StartTime = reader["StartTime"] != DBNull.Value ? (TimeSpan)reader["StartTime"] : null,
                                    EndTime = reader["EndTime"] != DBNull.Value ? (TimeSpan)reader["EndTime"] : null
                                };

                                // Move to the next result set for holidays
                                await reader.NextResultAsync();

                                var holidays = new List<string>();
                                while (await reader.ReadAsync())
                                {
                                    holidays.Add(reader["Day"].ToString());
                                }

                                doctorDetails.Holidays = holidays;

                                return new Tuple<clsDoctorDetailsDTO, string?>(doctorDetails, null);
                            }
                            else
                            {
                                return new Tuple<clsDoctorDetailsDTO, string?>(null, "Doctor not found");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsDoctorDetailsDTO, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<List<clsClinicDoctorDTO>, string?>?> GetClinicDoctorsByName(string doctorName, int pageSize, int pageNumber)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetClinicDoctorsByName", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DoctorName", doctorName);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsClinicDoctorDTO> doctors = new List<clsClinicDoctorDTO>();

                            while (await reader.ReadAsync())
                            {
                                var doctor = new clsClinicDoctorDTO
                                {
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorName = (string)reader["DoctorName"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"],
                                    DoctorImageLink = reader["DoctorImageLink"] != DBNull.Value ? (string)reader["DoctorImageLink"] : null,
                                    Gender = Convert.ToChar(reader["Gender"])
                                };

                                doctors.Add(doctor);
                            }

                            return new Tuple<List<clsClinicDoctorDTO>, string?>(doctors, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsClinicDoctorDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<List<clsClinicDoctorDTO>, string?>?> GetClinicDoctorsPreviousAppointmentService(int pageSize, int pageNumber, int patientID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetClinicDoctorsPriviousApponimentService", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PatientID", patientID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsClinicDoctorDTO> doctors = new List<clsClinicDoctorDTO>();

                            while (await reader.ReadAsync())
                            {
                                var doctor = new clsClinicDoctorDTO
                                {
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorName = (string)reader["DoctorName"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"],
                                    DoctorImageLink = reader["DoctorImageLink"] != DBNull.Value ? (string)reader["DoctorImageLink"] : null,
                                    Gender = Convert.ToChar(reader["Gender"])
                                };

                                doctors.Add(doctor);
                            }

                            return new Tuple<List<clsClinicDoctorDTO>, string?>(doctors, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsClinicDoctorDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }


        public static async Task<Tuple<List<clsClinicDoctorDTO>, string?>?> GetClinicDoctorsPatientFavorite(int pageSize, int pageNumber, int patientID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetClinicDoctorsPatientFavorite", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PatientID", patientID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<clsClinicDoctorDTO> doctors = new List<clsClinicDoctorDTO>();

                            while (await reader.ReadAsync())
                            {
                                var doctor = new clsClinicDoctorDTO
                                {
                                    DoctorID = (int)reader["DoctorID"],
                                    DoctorName = (string)reader["DoctorName"],
                                    SpecialtiesName = (string)reader["SpecialtiesName"],
                                    DoctorImageLink = reader["DoctorImageLink"] != DBNull.Value ? (string)reader["DoctorImageLink"] : null,
                                    Gender = Convert.ToChar(reader["Gender"])
                                };

                                doctors.Add(doctor);
                            }

                            return new Tuple<List<clsClinicDoctorDTO>, string?>(doctors, null);
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<List<clsClinicDoctorDTO>, string?>(null, EX.Message);
                    }
                }
            }
        }


        public static async Task<Tuple<bool, string?>?> UpdateCompletedDoctor(clsCompletedDoctorDTO CompletedDoctorDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateCompletedDoctor", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DoctorID", CompletedDoctorDTO.Doctor?.DoctorID);
                    command.Parameters.AddWithValue("@FirstName", CompletedDoctorDTO.Person?.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", CompletedDoctorDTO.Person?.MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", CompletedDoctorDTO.Person?.LastName);
                    command.Parameters.AddWithValue("@BirthDate", CompletedDoctorDTO.Person?.BirthDate);
                    command.Parameters.AddWithValue("@Gender", CompletedDoctorDTO.Person?.Gender);
                    command.Parameters.AddWithValue("@Nationality", CompletedDoctorDTO.Person?.Nationality);
                    command.Parameters.AddWithValue("@SpecialtiesName", CompletedDoctorDTO.Doctor?.SpecialtiesName);
                    command.Parameters.AddWithValue("@ConsultationFee", CompletedDoctorDTO.DoctorConsultation?.ConsultationFee);
                    command.Parameters.AddWithValue("@ConsultationDurationInMinutes", CompletedDoctorDTO.DoctorConsultation?.ConsultationDurationInMinutes);

                    string holidayDays = "";
                    if (CompletedDoctorDTO.Holidays != null && CompletedDoctorDTO.Holidays.Any())
                    {
                        holidayDays = string.Join(",", CompletedDoctorDTO.Holidays.Select(h => h.Day));
                    }
                    command.Parameters.AddWithValue("@HolidayDays", holidayDays);

                    command.Parameters.AddWithValue("@StartTime", CompletedDoctorDTO.DoctorWorkTimeDTO?.StartTime);
                    command.Parameters.AddWithValue("@EndTime", CompletedDoctorDTO.DoctorWorkTimeDTO?.EndTime);

                    try
                    {
                        await conn.OpenAsync();
                        var Result = await command.ExecuteNonQueryAsync();

                        if (Result > 0)
                        {
                            return new Tuple<bool, string?>(true, null);
                        }
                        else
                        {
                            return new Tuple<bool, string?>(false, "No rows affected");
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<bool, string?>(false, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<clsCompletedDoctorDTO, string?>?> FindDoctorByID(int doctorID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_FindDoctorByID", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorID", doctorID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var doctorDTO = new clsCompletedDoctorDTO
                                {
                                    Person = new clsPersonDTO
                                    {
                                        FirstName = (string)reader["FirstName"],
                                        MiddleName = reader["MiddleName"] != DBNull.Value ? (string)reader["MiddleName"] : null,
                                        LastName = (string)reader["LastName"],
                                        BirthDate = (DateTime)reader["BirthDate"],
                                        Gender = Convert.ToChar(reader["Gender"]),
                                        Nationality = (string)reader["Nationality"]
                                    },
                                    Doctor = new clsDoctorDTO
                                    {
                                        DoctorID = (int)reader["DoctorID"],
                                        SpecialtiesName = (string)reader["SpecialtiesName"]
                                    },
                                    DoctorConsultation = new clsDoctorConsultationDTO
                                    {
                                        ConsultationFee = Convert.ToSingle(reader["ConsultationFee"]),
                                        ConsultationDurationInMinutes = (short)reader["ConsultationDurationInMinutes"]
                                    },
                                    DoctorWorkTimeDTO = new clsDoctorWorkTimeDTO
                                    {
                                        StartTime = reader["StartTime"] != DBNull.Value ? (TimeSpan)reader["StartTime"] : null,
                                        EndTime = reader["EndTime"] != DBNull.Value ? (TimeSpan)reader["EndTime"] : null
                                    }
                                };

                                // Move to next result set for holidays
                                await reader.NextResultAsync();

                                var holidays = new List<clsDoctorHolidayDayDTO>();
                                while (await reader.ReadAsync())
                                {
                                    holidays.Add(new clsDoctorHolidayDayDTO
                                    {
                                        Day = (byte)reader["Day"]
                                    });
                                }
                                doctorDTO.Holidays = holidays;

                                return new Tuple<clsCompletedDoctorDTO, string?>(doctorDTO, null);
                            }
                            else
                            {
                                return new Tuple<clsCompletedDoctorDTO, string?>(null, "Doctor not found");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsCompletedDoctorDTO, string?>(null, EX.Message);
                    }
                }
            }
        }

    }
}