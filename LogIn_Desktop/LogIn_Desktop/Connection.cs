using Microsoft.Data.SqlClient;

namespace LogIn_Desktop
{
    class Connection
    {
        private string connectionString;

        public Connection()
        {
            this.SetConnectionString();
        }

        private void SetConnectionString()
        {
            this.connectionString = @"Server= localhost\SQLEXPRESS; Database= LoginDB;Integrated Security= True";
        }

        public SqlCommand Query(string query)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return new SqlCommand(query, connection);
        }

        public void Dispose()
        {
        }

    }
}
