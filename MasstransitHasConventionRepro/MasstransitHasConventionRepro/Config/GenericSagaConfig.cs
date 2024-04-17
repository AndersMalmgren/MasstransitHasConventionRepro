using MasstransitHasConventionRepro.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace MasstransitHasConventionRepro.Config
{
    public class GenericSagaConfig<TSaga, TState> : SagaClassMap<TSaga>, IEntityTypeConfiguration<TSaga> where TSaga : class, IGenericSaga<TState> where TState : class
    {
        private readonly JsonSerializerOptions _options = new() { WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public void Configure(EntityTypeBuilder<TSaga> builder)
        {
            builder.ToTable(typeof(TSaga).Name, "Saga");

            builder.Property(e => e.StateData)
                .HasConversion(
                    data => System.Text.Encoding.ASCII.GetBytes(JsonSerializer.Serialize(data, _options)),
                    data => JsonSerializer.Deserialize<TState>(System.Text.Encoding.ASCII.GetString(data), _options)!);

            builder.Property(e => e.CurrentState);
        }
    }
}