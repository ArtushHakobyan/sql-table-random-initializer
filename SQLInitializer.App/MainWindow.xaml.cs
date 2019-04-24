using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace SQLInitializer.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cmbTables.Items.Clear();
            SqlConnectionStringBuilder builer = new SqlConnectionStringBuilder()
            {
                DataSource = txtDataSource.Text,
                InitialCatalog = txtInitialCatalog.Text,
                IntegratedSecurity = chkSecurity.IsChecked ?? false
            };

            string connectionString = builer.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmdString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
                SqlCommand command = new SqlCommand(cmdString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbTables.Items.Add(reader["TABLE_SCHEMA"].ToString() + '.' + reader["TABLE_NAME"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    throw;
                }
            }
        }
    }
}
