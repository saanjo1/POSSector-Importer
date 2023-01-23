using Microsoft.EntityFrameworkCore;

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
