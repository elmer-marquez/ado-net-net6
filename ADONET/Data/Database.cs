
using ADONET.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ADONET.Data
{
    internal class Database
    {
        private readonly string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = new List<User>();

            try
            {
                using(SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    string query = "SELECT Id, Name, LastName, Email FROM Users";

                    using(SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();

                            adapter.Fill(dt);

                            users = dt.AsEnumerable().Select((row) => new User
                            {
                                Id = row.Field<int>("Id"),
                                Name = row.Field<string>("Name"),
                                LastName = row.Field<string>("LastName"),
                                Email = row.Field<string>("Email")

                            }).ToList();

                            //foreach (DataRow drow in dt.Rows)
                            //{
                            //    User user = new User
                            //    {
                            //        Id = Convert.ToInt32(drow["Id"]),
                            //        Name = drow["Name"].ToString(),
                            //        LastName = drow["LastName"].ToString(),
                            //        Email = drow["Email"].ToString()
                            //    };
                            //    users.Add(user);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener datos de usuarios: " + ex.ToString());
            }

            return users;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            bool saved = false;

            try
            {
                using(SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Users(Name, LastName, Email) VALUES(@name, @lastname, @email)";

                    conn.Open();

                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@name", user.Name));
                        cmd.Parameters.Add(new SqlParameter("@lastname", user.LastName));
                        cmd.Parameters.Add(new SqlParameter("@email", user.Email));

                        int affectedLines = await cmd.ExecuteNonQueryAsync();

                        if(affectedLines > 0) saved = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar usuario: {ex.Message}");
            }

            return saved;
        }

        public async Task<int> TotalRecordsAsync()
        {
            int records = 0;

            try
            {
                using(SqlConnection con = new SqlConnection(_connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Users";

                    con.Open();

                    using(SqlCommand cmd = new SqlCommand(query, con))
                    {
                        records = (int) await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la cantidad de registros: {ex.Message}");
            }

            return records;
        }
    }
}
