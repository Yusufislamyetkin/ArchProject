using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.EntityLayer.Entities;
using Arch.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ICompetitonService _competitonService;
        private readonly IFileService _fileService;
        private readonly IBlogService _blogService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public CompetitionController(ICompetitonService competitonService, UserManager<AppUser> userManager, IWebHostEnvironment hostingEnvironment, IBlogService blogService, IFileService fileService)
        {
            _competitonService = competitonService;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _blogService = blogService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        //Anasayfa
        public async Task<IActionResult> AllCompetitions(int type, int? status)
        {
            List<Competition> competitionsValue = new List<Competition>();

            if (status == null)
            {
                status = 2;
            }
            if (type == 0)
            {
                competitionsValue = await _competitonService.Where(x => x.Status == status).Include(x => x.DesignerUsers).ThenInclude(x => x.Designer).ToListAsync();
                return View(competitionsValue);
            }

            competitionsValue = await _competitonService.Where(x => x.Status == status && x.ProjectType == type).Include(x => x.DesignerUsers).ThenInclude(x => x.Designer).ToListAsync();
            return View(competitionsValue);
        }

        // Tipe Göre Filtreleme
        public async Task<IActionResult> FilterCompetitionsWithType(int Type)
        {
            var myvalue = await _competitonService.Where(x => x.Status == 2 && x.ProjectType == Type).Include(x => x.DesignerUsers).ThenInclude(x => x.Designer).ToListAsync();
            return Json(myvalue);
        }

        // Benim yarışmalarım
        public async Task<IActionResult> MyAllCompetitions(bool CreateStatus, int paymentStatus)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var myvalue = _competitonService.Where(x => x.CustomerId == user.Id).ToList();

            myvalue.Reverse();

            ViewData["CreateStatus"] = CreateStatus;
            ViewData["PaymentStatus"] = paymentStatus;

            return View(myvalue);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Create(CompetitonCreateDto createDto, List<IFormFile> files)
        {
            var value = await _userManager.FindByNameAsync(User.Identity.Name);
            createDto.CustomerId = value.Id;

        

            if (ModelState.IsValid)
            {
                var competition = await _competitonService.CreateCompetition(createDto);
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var webRootPath = _hostingEnvironment.WebRootPath;
                        var uploadsFolder = Path.Combine(webRootPath, "UserFiles");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString();
                        var fileExtension = Path.GetExtension(file.FileName);
                        var fileName = uniqueFileName + fileExtension;
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        ProjectFilePath projectFilePath = new ProjectFilePath();
                        projectFilePath.CompetitionId = competition.Id;
                        projectFilePath.Address = "/" + Path.Combine("UserFiles", fileName).Replace("\\", "/");

                        await _fileService.CreateFile(projectFilePath);
                    }
                }

                return RedirectToAction("MyAllCompetitions", new { CreateStatus = true });
            }

            return View(createDto);
        }

        // Devam eden yarışmaların listelendiği sayfa
        public async Task<IActionResult> ContinueCompetitions()
        {
            var activeCompetitions = await _competitonService.Where(x => x.Status == 2).Include(x => x.DesignerUsers).ThenInclude(x => x.Designer).ToListAsync();
            var viewModel = new CompetitionViewModel
            {
                ActiveCompetitions = activeCompetitions
            };
            return View(viewModel);
        }

        // Biten yarışmaların listelendiği sayfa
        public async Task<IActionResult> FinishedCompetitions()
        {
            var finishedCompetitions = await _competitonService.Where(x => x.Status == 4).Include(x => x.DesignerUsers).ThenInclude(x => x.Designer).ToListAsync();
            var viewModel = new CompetitionViewModel
            {
                FinishedCompetitions = finishedCompetitions
            };
            return View(viewModel);
        }
    }
}
