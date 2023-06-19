using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Concrete;
using Arch.EntityLayer.Entities;
using Arch.UI.Helper;
using Arch.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(IBlogService blogService, ICompetitonService competitonService, UserManager<AppUser> userManager, IFileService fileService, IDesignerUserService designerUserService, IWebHostEnvironment webHostEnvironment)
        {
            _blogService = blogService;
            _competitonService = competitonService;
            _userManager = userManager;
            _fileService = fileService;
            _designerUserService = designerUserService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogWithCompetitionId(int id, int paymentStatus)
        {
            #region Veri Çekme
            var blogComments = await _blogService.Where(x => x.CompetitionId == id).Include(x => x.Author).ToListAsync();
            blogComments.Reverse();
            var DataComp = await _competitonService.GetByIdAsync(id);
            ViewBag.UserCount = _designerUserService.Where(x => x.CompetitionId == id).Count();
            ViewBag.competition = DataComp;
            ViewBag.EndDate = DataComp.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
            #endregion

            #region View Dosyaları Ekleme 

            var files = await _fileService.GetByCompIdForView(id);

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

            FileFormat mainImage = fileFormats.FirstOrDefault(x => x.FileType == "image");

            if (mainImage == null || mainImage.Address == null)
            {
                mainImage = new FileFormat();
                mainImage.Address = "/UserFiles/mimari.jpg";
            }
            else
            {
                ViewBag.mainImage = mainImage.Address;
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.ProfilPhoto = user.ProfilPhoto;


            ViewBag.files = fileFormats;
            #endregion



            #region UploadDosyalarıYükleme

            var filesForTable = await _fileService.GetByCompIdForTable(id);

            var groupedFiles = filesForTable.GroupBy(file => new { file.DesignerId, file.Designer })
                 .Select(group => new DesignerFiles
                 {
                     DesignerId = group.Key.DesignerId,
                     Name = group.Key.Designer.ToString(),
                     Files = group.ToList(),
                 })
                 .ToList();

            ViewBag.groupedFiles = groupedFiles;






            #endregion

            #region Ödeme Bildirimi
            ViewData["PaymentStatus"] = paymentStatus;
            if (paymentStatus == 1)
            {
                DataComp.Status = 2;
                await _competitonService.UpdateAsync(DataComp);
                await _competitonService.UnitOfWorkAsync();
            }
            #endregion

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


        public IActionResult DownloadDesignerFiles(string designerName)
        {
            var designer = _userManager.FindByNameAsync(designerName).Result;


            var filesToDownload = _fileService.Where(file => file.DesignerId == designer.Id && file.Type == 1).ToList();

            var zipMemoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in filesToDownload)
                {
                    var designerFolderName = $"{designer.UserName}";
                    var entryFileName = Path.GetFileName(file.Address);
                    var entryPath = Path.Combine(designerFolderName, entryFileName);

                    var entry = zipArchive.CreateEntry(entryPath);

                    using (var entryStream = entry.Open())
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, file.Address.TrimStart('/'));

                        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
            }

            zipMemoryStream.Seek(0, SeekOrigin.Begin);

            var zipFileName = $"{designerName}_Files.zip";

            return File(zipMemoryStream, "application/octet-stream", zipFileName);
        }

        public async Task<IActionResult> DownloadCompetitionFiles(int competitionId)
        {
            var competition = await _competitonService.GetByIdAsync(competitionId);
            if (competition == null)
            {
                return NotFound();
            }

            var competitionFiles = await _fileService.GetByCompIdForTable(competitionId);

            var zipMemoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in competitionFiles)
                {
                    var designerFolderName = file.Designer.UserName;
                    var entryFileName = Path.GetFileName(file.Address);
                    var entryPath = Path.Combine(designerFolderName, entryFileName);

                    var entry = zipArchive.CreateEntry(entryPath);

                    using (var entryStream = entry.Open())
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, file.Address.TrimStart('/'));

                        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
            }

            zipMemoryStream.Seek(0, SeekOrigin.Begin);

            var zipFileName = $"{competition.Name}_Files.zip";

            return File(zipMemoryStream, "application/octet-stream", zipFileName);
        }


    }
}
