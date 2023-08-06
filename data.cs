using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace JosueLAssignment3991605014
{
    public class data
    {
        private static string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=WorldDB;Integrated Security=True";
        public static string ConnectionString { get => connStr; }

        public DataTable GetAllContinents()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Continent";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public DataTable GetCountriesByContinent(string continentName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Country WHERE ContinentId = @ContinentId";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ContinentId", GetContinentId(continentName));
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public DataTable GetCitiesByCountry(string countryName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM City WHERE CountryId = @CountryId";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CountryId", GetCountryId(countryName));
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public void InsertContinent(string continentName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "INSERT INTO Continent (ContinentName) VALUES (@ContinentName)";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ContinentName", continentName);
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertCountry(string continentName, string countryName, string language, string currency)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "INSERT INTO Country (ContinentId, CountryName, Language, Currency) VALUES (@ContinentId, @CountryName, @Language, @Currency)";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ContinentId", GetContinentId(continentName));
                    command.Parameters.AddWithValue("@CountryName", countryName);
                    command.Parameters.AddWithValue("@Language", language);
                    command.Parameters.AddWithValue("@Currency", currency);
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertCity(string countryName, string cityName, int population, bool isCapital)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "INSERT INTO City (CountryId, CityName, Population, IsCapital) VALUES (@CountryId, @CityName, @Population, @IsCapital)";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CountryId", GetCountryId(countryName));
                    command.Parameters.AddWithValue("@CityName", cityName);
                    command.Parameters.AddWithValue("@Population", population);
                    command.Parameters.AddWithValue("@IsCapital", isCapital);
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private int GetContinentId(string continentName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT ContinentId FROM Continent WHERE ContinentName = @ContinentName";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ContinentName", continentName);
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        throw new Exception("Continent not found");
                    }
                }
            }
        }

        private int GetCountryId(string countryName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT CountryId FROM Country WHERE CountryName = @CountryName";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@CountryName", countryName);
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        throw new Exception("Country not found");
                    }
                }
            }
        }
    }
}

