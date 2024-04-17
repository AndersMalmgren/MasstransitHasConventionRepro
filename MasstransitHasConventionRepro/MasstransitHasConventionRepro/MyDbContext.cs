using MassTransit.EntityFrameworkCoreIntegration;
using MasstransitHasConventionRepro.Config;
using MasstransitHasConventionRepro.Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MasstransitHasConventionRepro
{
    internal class MyDbContext : SagaDbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(MySagaState).Assembly);
            foreach (var config in Configurations) builder.ApplyConfiguration(_myConfig);
        }

        private readonly GenericSagaConfig<MySagaState, MySagaStateMachineData> _myConfig = new();
        protected override IEnumerable<ISagaClassMap> Configurations => new[] { _myConfig };
    }
}
