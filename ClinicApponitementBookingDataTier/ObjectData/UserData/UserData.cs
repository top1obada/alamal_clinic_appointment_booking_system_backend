using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingDataTier.Setting;
using ClinicAppointmentsBookingDTO.UserDTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicAppointmentBookingDataTier.ObjectData.UserData
{
    public static class clsUserData
    {
        public static async Task<Tuple<clsLoginDTO, string?>?> Login(string userName)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_Login", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserName", userName);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var loginInfo = new clsLoginDTO
                                {
                                    HashedPassword = (byte[])reader["HashedPassword"],
                                    Salt = (byte[])reader["Salt"],
                                    UserID = (int)reader["UserID"],
                                    JoiningDate = (DateTime)reader["JoiningDate"],
                                    PersonID = (int)reader["PersonID"],
                                    FirstName = (string)reader["FirstName"],
                                    UserRole = (enUserRole)Convert.ToByte( reader["UserRole"]),
                                    UserBranchID = reader["UserBranchID"] == DBNull.Value ?
                                    null : (int)reader["UserBranchID"]
                                };

                                return new Tuple<clsLoginDTO, string?>(loginInfo, null);
                            }
                            else
                            {
                                return new Tuple<clsLoginDTO, string?>(null, "User not found");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsLoginDTO, string?>(null, EX.Message);
                    }
                }
            }
        }

        public static async Task<Tuple<clsLoginDTO, string?>?> LoginByRefreshToken(string hashedRefreshToken)
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_LoginByRefreshToken", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@HashedRefreshToken", hashedRefreshToken);

                    try
                    {
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var loginInfo = new clsLoginDTO
                                {
                                    HashedPassword = (byte[])reader["HashedPassword"],
                                    Salt = (byte[])reader["Salt"],
                                    UserID = (int)reader["UserID"],
                                    JoiningDate = (DateTime)reader["JoiningDate"],
                                    PersonID = (int)reader["PersonID"],
                                    FirstName = (string)reader["FirstName"],
                                    UserRole = (enUserRole)Convert.ToByte(reader["UserRole"]),
                                    UserBranchID = reader["UserBranchID"] == DBNull.Value ?
                                    null : (int)reader["UserBranchID"]
                                };

                                return new Tuple<clsLoginDTO, string?>(loginInfo, null);
                            }
                            else
                            {
                                return new Tuple<clsLoginDTO, string?>(null, "Invalid or revoked refresh token");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        return new Tuple<clsLoginDTO, string?>(null, EX.Message);
                    }
                }
            }
        }
    }
}