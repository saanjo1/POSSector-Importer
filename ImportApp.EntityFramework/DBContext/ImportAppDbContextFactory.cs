using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.EntityFramework.DBContext
{
    public class ImportAppDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _optionsBuilder;

        public ImportAppDbContextFactory(Action<DbContextOptionsBuilder> optionsBuilder)
        {
            _optionsBuilder = optionsBuilder;
        }

        public ImportAppDbContext CreateDbContext(string[]? args = null)
        { 
            DbContextOptionsBuilder<ImportAppDbContext> options = new DbContextOptionsBuilder<ImportAppDbContext>();

            _optionsBuilder(options);

            return new ImportAppDbContext(options.Options);
        }
    }
}
