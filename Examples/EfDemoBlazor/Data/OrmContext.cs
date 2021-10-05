using BIT.Data.Sync;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BIT.EfCore.Sync;

namespace EfDemoOrm
{
    public class OrmContext : SyncFrameworkDbContext
    {
        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="OrmContext" /> class. The
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />
        /// method will be called to configure the database (and other options) to be used for this context.
        /// </para>
        /// </summary>
        protected OrmContext()
        {

        }

        protected OrmContext(DbContextOptions options, IServiceCollection ServiceCollection, string Identity) : base(options, ServiceCollection, Identity)
        {

        }

        public OrmContext(DbContextOptions options, IServiceCollection ServiceCollection, ISyncIdentityService syncIdentityService) : base(options, ServiceCollection, syncIdentityService)
        {

        }

        public DbSet<Blog> Blogs { get; set; }
    }
}
