using Core.Models;
using Microsoft.AspNetCore.Mvc;
using NewsLater.Reporting.Api.Features;

namespace NewsLater.Reporting.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticlesController(ArticleService articleService) =>
            _articleService = articleService;


        [HttpPost]
        public async Task<IActionResult> Post(Article newArticle)
        {
            await _articleService.CreateAsync(newArticle);

            return Ok();
        }


        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            return await _articleService.GetAsync();
        }
    }
}
