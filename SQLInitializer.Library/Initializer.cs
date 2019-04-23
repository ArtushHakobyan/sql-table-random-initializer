using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SQLInitializer.Library
{
    public static class Initializer
    {
        private static Random rd = new Random();

        public static void Initialize(int count, string connectionString, string table)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DataTable tbl = GetDataTable(connectionString, table);
                    string cmdString = string.Empty;
                    SqlCommand command;

                    for (int i = 0; i < count; i++)
                    {
                        cmdString = $"INSERT INTO {table} VALUES({GetValues(tbl)})";
                        command = new SqlCommand(cmdString, connection);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private static DataTable GetDataTable(string connectionString, string table)
        {
            DataTable tbl = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM {table}", connection);
                SqlDataReader reader = command.ExecuteReader();
                tbl.Load(reader);
            }

            return tbl;
        }

        private static string GetValues(DataTable table)
        {
            string values = string.Empty;
            string dataType = string.Empty;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                dataType = table.Columns[i].DataType.ToString();

                switch (dataType)
                {
                    case "System.Boolean":
                        values += rd.Next(0, 2) + ", ";
                        break;
                    case "System.DateTime":
                        values += $"'{rd.Next(1990, 2018)}-{rd.Next(0, 13)}-{rd.Next(0, 32)}', ";
                        break;
                    case "System.Int32":
                        values += $"{rd.Next(-200000, 200001)}";
                        break;
                }
            }

            return values;
        }
    }
}