using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HospitalAllocation.Model
{
    /// <summary>
    /// A factory for <see cref="T:HospitalAllocation.Model.AllocationContext"/>
    /// during design time.
    /// </summary>
    public class DesignTimeAllocationContextFactory : IDesignTimeDbContextFactory<AllocationContext>
    {
        /// <summary>
        /// Creates an <see cref="T:HospitalAllocation.Model.AllocationContext"/>.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>An <see cref="T:HospitalAllocation.Model.AllocationContext"/>.</returns>
        public AllocationContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AllocationContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

            return new AllocationContext(optionsBuilder.Options);
        }
    }
}
