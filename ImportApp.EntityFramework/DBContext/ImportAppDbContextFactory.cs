using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.EntityFramework.DBContext
{
    public class ImportAppDbContextFactory : IDesignTimeDbContextFactory<ImportAppDbContext>
    {
        public ImportAppDbContext CreateDbContext(string[]? args = null)
        {
            var options = new DbContextOptionsBuilder<ImportAppDbContext>();

            options.UseSqlServer("Server=.;Database=possector;Trusted_Connection=True;Encrypt=False");

            return new ImportAppDbContext(options.Options);
        }
    }
}
