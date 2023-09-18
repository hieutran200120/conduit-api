using ConduitAPI.Services.Articles;
using ConduitAPI.Services.Articles.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ConduitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGlobalArticle([FromQuery] QueryGlobalArticleRequestDto query) 
        {
            var response = await _articleService.GetGlobalArticle(query);
            return Ok(response);
        }
    }
}
