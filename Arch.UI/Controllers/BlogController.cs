using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Concrete;
using Arch.EntityLayer.Entities;
using Arch.UI.Helper;
using Arch.UI.Models;
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
        private readonly IFileService _fileService;
        private readonly IDesignerUserService _designerUserService;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(IBlogService blogService, ICompetitonService competitonService, UserManager<AppUser> userManager, IFileService fileService, IDesignerUserService designerUserService)
        {
            _blogService = blogService;
            _competitonService = competitonService;
            _userManager = userManager;
            _fileService = fileService;
            _designerUserService = designerUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogWithCompetitionId(int id)
        {

            var blogComments = await _blogService.Where(x => x.CompetitionId == id).Include(x => x.Author).ToListAsync();

            blogComments.Reverse();
            var DataComp = await _competitonService.GetByIdAsync(id);
            ViewBag.UserCount = _designerUserService.Where(x => x.CompetitionId == id).Count();
            ViewBag.competition = DataComp;
            ViewBag.EndDate = DataComp.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            var files = await _fileService.GetByCompId(id);

            List<FileFormat> ff = new List<FileFormat>();
            foreach (var record in files)
            {
                FileFormat file = new FileFormat
                {
                    Address = record.Address,
                    FileName = FormatHelper.GetFileName(record.Address),
                    FileType = FormatHelper.GetFileType(record.Address)
                };

                ff.Add(file);
            }

            var fileFormats = ff.ToList();
       
            var mainImage = fileFormats.Where(x => x.FileType == "image").FirstOrDefault().Address;

            if (mainImage == null)
            {
                mainImage = "/UserFiles/mimari.jpg";
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.ProfilPhoto = user.ProfilPhoto;

            ViewBag.mainImage = mainImage;
            ViewBag.files = fileFormats;

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

        public async Task<IActionResult> CountDown()
        {
            return View();
        }
    }
}
