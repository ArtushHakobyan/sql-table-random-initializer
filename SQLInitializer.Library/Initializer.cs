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

        public static int Initialize(int count, string connectionString, string table)
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

                    return count;
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
                    case "System.Byte":
                        values += rd.NextByte() + ", ";
                        break;
                    case "System.Char":
                        values += rd.NextChar() + ", ";
                        break;
                    case "System.DateTime":
                        values += $"'{rd.NextDateTime()}', ";
                        break;
                    case "System.Decimal":
                        values += rd.NextDecimal() + ", ";
                        break;
                    case "System.Double":
                        values += rd.NextDouble() + ", ";
                        break;
                    case "System.Int32":
                        values += rd.Next(-200000, 200001) + ", ";
                        break;
                    case "System.String":
                        values += $"'{rd.NextString(rd.Next(6, 9), false)}', ";
                        break;
                    case "System.TimeSpan":
                        values += rd.NextTimeSpan() + ", ";
                        break;
                    case "System.Guid":
                        values += $"'{rd.NextGuid()}', ";
                        break;
                }
            }

            values = values.Substring(0, values.Length - 2);

            return values;
        }
    }
}