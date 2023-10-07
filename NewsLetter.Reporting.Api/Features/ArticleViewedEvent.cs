using Core.Contracts;
using MassTransit;

namespace NewsLater.Reporting.Api.Features
{
    public sealed class ArticleViewedEvent : IConsumer<ArticleVieweddEvent>
    {
        private readonly ArticleService _articleService;

        public ArticleViewedEvent(ArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task Consume(ConsumeContext<ArticleVieweddEvent> context)
        {
            var article = await _articleService.GetAsync(context.Message.Id);
            if (article is null)
            {
                return;
            }

            article.CountView = context.Message.CountView;

            await _articleService.UpdateAsync(context.Message.Id, article);
        }
    }
}
