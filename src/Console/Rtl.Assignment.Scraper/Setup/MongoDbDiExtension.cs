namespace Rtl.Assignment.Scraper.Setup
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Serializers;
    using Rtl.Assignment.Domain.Repositories;
    using Rtl.Assignment.Infrastructure.DataPersistence.Configuration;
    using Rtl.Assignment.Infrastructure.Repositories;

    public static class MongoDbDiExtension
    {
        public static ServiceCollection RegisterMongoDb(this ServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RtlDatabaseSettings>(
                configuration.GetSection(nameof(RtlDatabaseSettings)));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<RtlDatabaseSettings>>()?.Value ??
                throw new ArgumentNullException(nameof(RtlDatabaseSettings)));

            services.AddSingleton<IShowWithCastRepository, ShowWithCastRepository>();

            BsonSerializer.RegisterSerializer(DateTimeSerializer.DateOnlyInstance);

            return services;
        }
    }
}