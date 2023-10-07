using Core.Contracts;
using Core.Models;
using MassTransit;

namespace NewsLater.Reporting.Api.Features
{
    public sealed class ArticleCreatedConsumer : IConsumer<ArticleCreatedEvent>
    {
        private readonly ArticleService _articleService;

        public ArticleCreatedConsumer(ArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task Consume(ConsumeContext<ArticleCreatedEvent> context)
        {
            var article = new Article
            {
                Id = context.Message.Id,
                CreaDateTime = context.Message.DateCreated,
                Name = context.Message.Name,
            };


            await _articleService.CreateAsync(article);
        }
    }
}
