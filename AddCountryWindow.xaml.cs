using Microsoft.Data.SqlClient;
using System.Windows;
using System.Data;
using System.Windows.Markup;

namespace JosueLAssignment3991605014
{
    public partial class AddCountryWindow : Window
    {
        public AddCountryWindow()
        {
            InitializeComponent();
            LoadContinents();
        }

        public void LoadContinents()
        {
            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Continent", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                comboBoxContinent.ItemsSource = dt.DefaultView;
                comboBoxContinent.DisplayMemberPath = "ContinentName";
                comboBoxContinent.SelectedValuePath = "ContinentId";
                connection.Close();
            }
        }

        private void buttonAddCountry_Click(object sender, RoutedEventArgs e)
        {
            int continentId = (int)comboBoxContinent.SelectedValue;
            string countryName = textBoxCountryName.Text.Trim();
            string language = textBoxLanguage.Text.Trim();
            string currency = textBoxCurrency.Text.Trim();

            if (continentId <= 0 || string.IsNullOrEmpty(countryName))
            {
                labelValidationMessage.Content = "Continent and country names are required.";
                return;
            }

            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Country (ContinentId, CountryName, Language, Currency) VALUES (@ContinentId, @CountryName, @Language, @Currency)", connection);
                command.Parameters.AddWithValue("@ContinentId", continentId);
                command.Parameters.AddWithValue("@CountryName", countryName);
                command.Parameters.AddWithValue("@Language", language);
                command.Parameters.AddWithValue("@Currency", currency);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            comboBoxContinent.SelectedIndex = -1;
            textBoxCountryName.Text = "";
            textBoxLanguage.Text = "";
            textBoxCurrency.Text = "";

            labelValidationMessage.Content = "Country added successfully.";
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
