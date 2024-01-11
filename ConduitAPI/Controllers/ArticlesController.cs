using ConduitAPI.Services.Articles;
using ConduitAPI.Services.Articles.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
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
		[HttpGet("{slug}")]
		public async Task<IActionResult> GetArticle( string slug)
		{
			var response = await _articleService.GetArticle(slug);
			return Ok(response);
		}
    
        [HttpPost] 
        
        public async Task<IActionResult> PostArticle([FromBody] PostArticleDto request)
        {
            var res= await _articleService.PostArticle(request);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateArticle( string Slug, UpdateArticleDto request)
        {
            var res = await _articleService.UpdateArticle(Slug,request);
            return Ok(res);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteArticle(string Slug)
        {
            var res =await _articleService.DeleteArticle(Slug);
            return Ok(res);
        }
        
	}
}
