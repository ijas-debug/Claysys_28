using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using FinalProject.Models;
using System.IO;
using System.Reflection;

namespace FinalProject.Repository
{
    public class EVisaRepository
    {

        String connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ToString();

        public List<EVisa> GetAllEVisas()
            {
                List<EVisa> eVisas = new List<EVisa>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM E_Visa";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        EVisa eVisa = new EVisa
                        {
                            ApplicationID = (int)reader["ApplicationID"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            DateOfBirth = (DateTime)reader["DateOfBirth"],
                            VisaService = (string)reader["VisaService"],
                            Gender = (string)reader["Gender"],
                            Nationality = (string)reader["Nationality"],
                            PassportNumber = (string)reader["PassportNumber"],
                            Photo = (byte[])reader["Photo"],
                            ETANumber = (string)reader["ETANumber"],
                            NoOfEntries = (int)reader["NoOfEntries"],
                            DateOfIssueOfETA = (DateTime)reader["DateOfIssueOfETA"],
                            DateOfExpiryOfETA = (DateTime)reader["DateOfExpiryOfETA"],
                            ApplicationStatus = (string)reader["ApplicationStatus"]
                        };

                        eVisas.Add(eVisa);
                    }

                    connection.Close();
                }

                return eVisas;
            }

            public void AddEVisa(EVisa eVisa)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO E_Visa (ApplicationID, FirstName, LastName, DateOfBirth, VisaService, Gender, Nationality, PassportNumber, Photo, ETANumber, NoOfEntries, DateOfIssueOfETA, DateOfExpiryOfETA, ApplicationStatus) " +
                                   "VALUES (@ApplicationID, @FirstName, @LastName, @DateOfBirth, @VisaService, @Gender, @Nationality, @PassportNumber, @Photo, @ETANumber, @NoOfEntries, @DateOfIssueOfETA, @DateOfExpiryOfETA, @ApplicationStatus)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ApplicationID", eVisa.ApplicationID);
                    command.Parameters.AddWithValue("@FirstName", eVisa.FirstName);
                    command.Parameters.AddWithValue("@LastName", eVisa.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", eVisa.DateOfBirth);
                    command.Parameters.AddWithValue("@VisaService", eVisa.VisaService);
                    command.Parameters.AddWithValue("@Gender", eVisa.Gender);
                    command.Parameters.AddWithValue("@Nationality", eVisa.Nationality);
                    command.Parameters.AddWithValue("@PassportNumber", eVisa.PassportNumber);
                    command.Parameters.AddWithValue("@Photo", eVisa.Photo);
                    command.Parameters.AddWithValue("@ETANumber", eVisa.ETANumber);
                    command.Parameters.AddWithValue("@NoOfEntries", eVisa.NoOfEntries);
                    command.Parameters.AddWithValue("@DateOfIssueOfETA", eVisa.DateOfIssueOfETA);
                    command.Parameters.AddWithValue("@DateOfExpiryOfETA", eVisa.DateOfExpiryOfETA);
                    command.Parameters.AddWithValue("@ApplicationStatus", eVisa.ApplicationStatus);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


}
