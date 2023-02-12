using Microsoft.EntityFrameworkCore;

namespace ImportApp.EntityFramework.DBContext
{
    public class ImporterDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _optionsBuilder;

        public ImporterDbContextFactory(Action<DbContextOptionsBuilder> optionsBuilder)
        {
            _optionsBuilder = optionsBuilder;
        }

        public ImporterDbContext CreateDbContext(string[]? args = null)
        {
            DbContextOptionsBuilder<ImporterDbContext> options = new DbContextOptionsBuilder<ImporterDbContext>();

            _optionsBuilder(options);

            return new ImporterDbContext(options.Options);
        }
    }
}
