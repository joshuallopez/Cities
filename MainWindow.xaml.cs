using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace JosueLAssignment3991605014
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadContinents();
        }
        public void LoadContinents()
        {
            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Continent", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboBoxContinents.ItemsSource = dt.DefaultView;
                comboBoxContinents.DisplayMemberPath = "ContinentName";
                comboBoxContinents.SelectedValuePath = "ContinentId";
            }
        }
        private void comboBoxContinents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedContinent = (int)comboBoxContinents.SelectedValue;

            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Country WHERE ContinentId = @ContinentId", connection);
                command.Parameters.AddWithValue("@ContinentId", selectedContinent);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                listBoxCountries.ItemsSource = dt.DefaultView;
                listBoxCountries.DisplayMemberPath = "CountryName";
                listBoxCountries.SelectedValuePath = "CountryId";
            }
        }
        private void listBoxCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedCountry = (int)listBoxCountries.SelectedValue;

            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                // Load cities
                SqlCommand command = new SqlCommand("SELECT * FROM City WHERE CountryId = @CountryId", connection);
                command.Parameters.AddWithValue("@CountryId", selectedCountry);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridCities.ItemsSource = dt.DefaultView;

                // Load language and currency
                command = new SqlCommand("SELECT Language, Currency FROM Country WHERE CountryId = @CountryId", connection);
                command.Parameters.AddWithValue("@CountryId", selectedCountry);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    labelLanguages.Content = "Language: " + (string)reader["Language"];
                    labelCurrency.Content = "Currency: " + (string)reader["Currency"];
                }
                connection.Close();
            }
        }
        private void buttonAddContinent_Click(object sender, RoutedEventArgs e)
        {
            AddContinentWindow addContinentWindow = new AddContinentWindow();
            addContinentWindow.Owner = this;
            addContinentWindow.ShowDialog(); 
        }

        private void buttonAddCountry_Click(object sender, RoutedEventArgs e)
        {
            AddCountryWindow addCountryWindow = new AddCountryWindow();
            addCountryWindow.Owner = this;
            addCountryWindow.ShowDialog();
        }

        private void buttonAddCity_Click(object sender, RoutedEventArgs e)
        {
            AddCityWindow addCityWindow = new AddCityWindow();
            addCityWindow.Owner = this;
            addCityWindow.ShowDialog();
        }
    }
}

