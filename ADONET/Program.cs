
using ADONET.Data;
using ADONET.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADONET
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args).Build();

            var configuration = host.Services.GetService<IConfiguration>();

            //string strcnn = configuration.GetValue<string>("ConnectionString");
            string strcnn = configuration.GetConnectionString("SQLServerDB");

           Database db = new Database(strcnn);
            //bool saved = await db.AddUserAsync(new User
            //{
            //    Name = "Gloria",
            //    LastName = "Trevi",
            //    Email = "trevig@outlook.com"
            //});

            //Console.WriteLine($"Se guardo el usuario? {saved}");

            //int totalRecordsOnUsers = await db.TotalRecordsAsync();

            //Console.WriteLine($"Total de registros en la tabla Users: {totalRecordsOnUsers}");

            List<User> users = await db.GetAllUsersAsync();

            foreach (User user in users)
            {
                Console.WriteLine(user);
            }

            Console.WriteLine();

            await host.RunAsync();

        }
    }
}
