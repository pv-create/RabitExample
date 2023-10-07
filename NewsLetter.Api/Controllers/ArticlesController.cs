using Core.Contracts;
using Core.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsLater.Api.DataBase;

namespace NewsLater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public ArticlesController(
            ApplicationDbContext context,
            IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            return await _context.Articles.ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            var article = await _context.Articles.FindAsync(id);




            if (article == null)
            {
                return NotFound();
            }

            article.CountView++;

            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new ArticleVieweddEvent
            {
                Id = article.Id,
                DateViewed = DateTime.Now,
                CountView = article.CountView
            });

            return article;
        }



        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            if (_context.Articles is null)
            {
                return Problem("Entity set 'ApplicationDbContext.Articles'  is null.");
            }
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new ArticleCreatedEvent
            {
                Id = article.Id,
                DateCreated = article.CreaDateTime,
                Name = article.Name

            });

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }
    }
}
