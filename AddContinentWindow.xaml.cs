using Microsoft.Data.SqlClient;
using System.Windows;

namespace JosueLAssignment3991605014
{
    public partial class AddContinentWindow : Window
    {
        public AddContinentWindow()
        {
            InitializeComponent();
        }

        private void buttonAddContinent_Click(object sender, RoutedEventArgs e)
        {
            string continentName = textBoxContinentName.Text.Trim();
            if (string.IsNullOrEmpty(continentName))
            {
                labelValidationMessage.Content = "Continent name is required.";
                return;
            }

            using (SqlConnection connection = new SqlConnection(data.ConnectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Continent (ContinentName) VALUES (@ContinentName)", connection);
                command.Parameters.AddWithValue("@ContinentName", continentName);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            textBoxContinentName.Text = "";
            labelValidationMessage.Content = "Continent added successfully.";

            ((MainWindow)this.Owner).LoadContinents();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
