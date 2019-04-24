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
using SQLInitializer.Library;

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

        private void BtnInitialize_Click(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder builer = new SqlConnectionStringBuilder()
            {
                DataSource = txtDataSource.Text,
                InitialCatalog = txtInitialCatalog.Text,
                IntegratedSecurity = chkSecurity.IsChecked ?? false
            };

            string connectionString = builer.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SQLInitializer.Library.Initializer.Initialize(int.Parse(txtCount.Text), connectionString, cmbTables.SelectedValue.ToString());
                string cmdString = $"SELECT TOP(100) * FROM {cmbTables.SelectedItem.ToString()}";
                SqlCommand command = new SqlCommand(cmdString, connection);
                DataTable table = LoadTable(connectionString, cmbTables.SelectedValue.ToString());
                grdResult.DataContext = table.DefaultView;

                try
                {
                    connection.Open();

                    //for (int i = 0; i < table.Rows.Count; i++)
                    //{
                    //    lstResult.Items.Add(table.Rows[i]);
                    //}
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    throw;
                }
            }
        }

        private void AddTableColumns(string connectionString, string table)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmdString =
                    $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = '{table.Substring(4)}'";
                SqlCommand command = new SqlCommand(cmdString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    var gridView = new GridView();

                    while (reader.Read())
                    {
                        gridView.Columns.Add(new GridViewColumn { Header = reader[0].ToString() });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    throw;
                }
            }
        }

        private DataTable LoadTable(string connectionString, string table)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmdString = $"SELECT * FROM {table}";
                SqlCommand command = new SqlCommand(cmdString, connection);
                DataTable tbl = new DataTable();
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    tbl.Load(reader);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    throw;
                }

                return tbl;
            }
        }
    }
}
