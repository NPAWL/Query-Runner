using System;
using System.Data.SqlClient;

namespace QueryRunner.Helpers {
    public class DatabaseHandler {
        private static String connString = "";

        public static void Setup(string connString) {
            DatabaseHandler.connString = connString;
        }

        public static void PerformNonQuery(string sql) {
            using (SqlConnection connection = new SqlConnection(connString)) {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static SqlDataReader PerformQuery(string sql) {
            using (SqlConnection connection = new SqlConnection(connString)) {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                return command.ExecuteReader();;
            }
        }

        public static Boolean hasConnection() {
            return connString != "";
        }
    }
}