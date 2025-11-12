using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using ClinicAppointmentsBookingDTO.PersonDTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicAppointmentBookingDataTier.ObjectData.PersonData
{
    public static class clsPersonData
    {
        public static async Task<Tuple<clsPersonDTO, string?>?> GetPersonInfo(int personID)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonInfo", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", personID);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var personInfo = new clsPersonDTO
                                {
                                    PersonID = (int)reader["PersonID"],
                                    FirstName = (string)reader["FirstName"],
                                    MiddleName = reader["MiddleName"] != DBNull.Value ? (string)reader["MiddleName"] : null,
                                    LastName = (string)reader["LastName"],
                                    BirthDate = (DateTime)reader["BirthDate"],
                                    Gender = Convert.ToChar(reader["Gender"]),
                                    Nationality = (string)reader["Nationality"]
                                };

                                return new Tuple<clsPersonDTO, string?>(personInfo, null);
                            }
                            else
                            {
                                return new Tuple<clsPersonDTO, string?>(null, "Person not found");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsPersonDTO, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<string?> UpdatePerson(clsPersonDTO personDTO)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdatePerson", conn))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", personDTO.PersonID);
                    command.Parameters.AddWithValue("@FirstName", personDTO.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", personDTO.MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", personDTO.LastName);
                    command.Parameters.AddWithValue("@BirthDate", personDTO.BirthDate);
                    command.Parameters.AddWithValue("@Gender", personDTO.Gender);
                    command.Parameters.AddWithValue("@Nationality", personDTO.Nationality);

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

    }
}