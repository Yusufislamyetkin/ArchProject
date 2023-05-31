using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Concrete;
using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICompetitonService _competitonService;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(IBlogService blogService, ICompetitonService competitonService, UserManager<AppUser> userManager)
        {
            _blogService = blogService;
            _competitonService = competitonService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetBlogWithCompetitionId(int id)
        {
            var blogComments = await _blogService.Where(x => x.CompetitionId == id).Include(x => x.Author).ToListAsync();
            blogComments.Reverse();
            ViewBag.competition = await _competitonService.GetByIdAsync(id);

            return View(blogComments);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BlogPost blogPost)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            blogPost.AuthorId = user.Id;
            blogPost.Title = "Yarışma Yorum";
            blogPost.CreatedDate = DateTime.Now;

            await _blogService.AddAsync(blogPost);

            var userName = user.UserName;
            var timeAgo = Arch.EntityLayer.StaticClass.TimeAgo.GetTimeAgo(blogPost.CreatedDate);

            return Json(new { success = true, userName, timeAgo, commentText = blogPost.Content });
        }


    }
}
