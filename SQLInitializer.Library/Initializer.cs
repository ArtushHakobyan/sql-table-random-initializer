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
        public static string _connectionString = string.Empty;
        public static string _table = string.Empty;

        private static DataSet dataSet;
        private static Random rd = new Random();

        public static void Initialize(int count)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    DataTable table = GetDataTable();
                    string cmdString =
                        $"INSERT INTO {_table} VALUES({GetValues(table)})";
                    SqlCommand command = new SqlCommand(cmdString);

                    for (int i = 0; i < count; i++)
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private static DataTable GetDataTable()
        {
            DataTable table = new DataTable();
            SqlCommand command = new SqlCommand($"SELECT * FROM {_table}");
            SqlDataReader reader = command.ExecuteReader();
            table.Load(reader);

            return table;
        }

        private static string GetValues(DataTable table)
        {
            string values = string.Empty;
            string dataType = string.Empty;

            for(int i = 0; i < table.Columns.Count; i++)
            {
                dataType = table.Columns[i].DataType.ToString();

                switch (dataType)
                {
                    case "System.Boolean":
                        values += rd.Next(0, 2) + ", ";
                        break;
                    case "System.DateTime":
                        values += $"{rd.Next(1990, 2018)}-{rd.Next(0, 13)}-{rd.Next(0, 32)}, ";
                        break;
                }
            }

            return values;
        }
    }
}
