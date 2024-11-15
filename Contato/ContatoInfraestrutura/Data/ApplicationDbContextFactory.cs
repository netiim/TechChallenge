using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var sqlConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING");
        optionsBuilder.UseSqlServer(sqlConnectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
