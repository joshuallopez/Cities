using Microsoft.Data.SqlClient;
using System.Windows;
using System.Data;
using System.Windows.Markup;

namespace JosueLAssignment3991605014
{
    public partial class AddCityWindow : Window
    {
        public AddCityWindow()
        {
            InitializeComponent();
            LoadCountries();
        }

        public void LoadCountries()
        {
            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT CountryName FROM Country", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                comboBoxCountry.ItemsSource = dt.DefaultView;
                comboBoxCountry.DisplayMemberPath = "CountryName";
                connection.Close();
            }
        }

        private void buttonAddCity_Click(object sender, RoutedEventArgs e)
        {
            string countryName = comboBoxCountry.Text;
            string cityName = textBoxCityName.Text.Trim();
            bool isCapital = checkBoxCapital.IsChecked.Value;
            int population;

            if (!int.TryParse(textBoxPopulation.Text, out population))
            {
                labelValidationMessage.Content = "Invalid population.";
                return;
            }

            if (string.IsNullOrEmpty(countryName) || string.IsNullOrEmpty(cityName))
            {
                labelValidationMessage.Content = "Country and city names are required.";
                return;
            }

            int countryId;
            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT CountryId FROM Country WHERE CountryName = @CountryName", connection);
                command.Parameters.AddWithValue("@CountryName", countryName);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    countryId = reader.GetInt32(0);
                }
                else
                {
                    labelValidationMessage.Content = "Invalid country name.";
                    return;
                }
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO City (CountryId, CityName, Population, IsCapital) VALUES (@CountryId, @CityName, @Population, @IsCapital)", connection);
                command.Parameters.AddWithValue("@CountryId", countryId);
                command.Parameters.AddWithValue("@CityName", cityName);
                command.Parameters.AddWithValue("@Population", population);
                command.Parameters.AddWithValue("@IsCapital", isCapital);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }


            comboBoxCountry.SelectedIndex = -1;
            textBoxCityName.Text = "";
            textBoxPopulation.Text = "";
            checkBoxCapital.IsChecked = false;

            labelValidationMessage.Content = "City added successfully.";
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
