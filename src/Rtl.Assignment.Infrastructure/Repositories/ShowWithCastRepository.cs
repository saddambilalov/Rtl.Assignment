namespace Rtl.Assignment.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using Rtl.Assignment.Domain.Entities;
    using Rtl.Assignment.Domain.Repositories;
    using Rtl.Assignment.Infrastructure.DataPersistence.Configuration;

    public class ShowWithCastRepository : IShowWithCastRepository
    {
        private readonly IMongoCollection<ShowWithCastEntity> showWithCast;
        private readonly RtlDatabaseSettings rtlDatabaseSettings;

        public ShowWithCastRepository(RtlDatabaseSettings rtlDatabaseSettings)
        {
            this.rtlDatabaseSettings = rtlDatabaseSettings;

            var client = new MongoClient(rtlDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(rtlDatabaseSettings.DatabaseName);

            this.showWithCast = database.GetCollection<ShowWithCastEntity>(
                rtlDatabaseSettings.ShowWithCastSettings.CollectionName);
        }

        public async Task<IEnumerable<ShowWithCastEntity>> GetAllAsync(int page, CancellationToken token)
        {
            var pageSize = this.rtlDatabaseSettings.ShowWithCastSettings.PageSize;

            return await this.showWithCast
                .Find(_ => true)
                .Skip(page * pageSize)
                .Limit(pageSize)
                .ToListAsync(token);
        }

        public async Task UpdateShowBulkAsync(
            IEnumerable<ShowWithCastEntity> replacements,
            CancellationToken token)
        {
            var bulkOps = replacements.Select(replacement =>
                new ReplaceOneModel<ShowWithCastEntity>(
                    Builders<ShowWithCastEntity>
                    .Filter.Where(x => x.Id == replacement.Id), replacement)
                {
                    IsUpsert = true,
                })
                .Cast<WriteModel<ShowWithCastEntity>>().ToList();
            await this.showWithCast.BulkWriteAsync(bulkOps, cancellationToken: token);
        }

        public async Task UpdateCastAsync(int id, ShowWithCastEntity replacement, CancellationToken token)
        {
            var filter = Builders<ShowWithCastEntity>.Filter.Where(_ => _.Id == id);
            var update = Builders<ShowWithCastEntity>.Update
                .Set(_ => _.Cast, replacement.Cast);

            await this.showWithCast.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<ShowWithCastEntity>
            {
                IsUpsert = true,
            }, token);
        }
    }
}