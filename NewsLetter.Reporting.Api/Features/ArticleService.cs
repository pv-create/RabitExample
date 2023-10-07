using Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NewsLater.Reporting.Api.DataBase;

namespace NewsLater.Reporting.Api.Features
{
    public class ArticleService
    {
        private readonly IMongoCollection<Article> _articleCollection;

        public ArticleService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _articleCollection = mongoDatabase.GetCollection<Article>(
                bookStoreDatabaseSettings.Value.ArticlesCollectionName);
        }

        public async Task CreateAsync(Article newArticle) =>
            await _articleCollection.InsertOneAsync(newArticle);



        public async Task<List<Article>> GetAsync() =>
            await _articleCollection.Find(_ => true).ToListAsync();

        public async Task<Article?> GetAsync(int id) =>
            await _articleCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(int id, Article updatedArticle) =>
            await _articleCollection.ReplaceOneAsync(x => x.Id == id, updatedArticle);
    }
}
