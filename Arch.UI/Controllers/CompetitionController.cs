using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.EntityLayer.Entities;
using Arch.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ICompetitonService _competitonService;
        private readonly IBlogService _blogService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private const int CompetitionsPerPage = 12;

        public CompetitionController(ICompetitonService competitonService, UserManager<AppUser> userManager, IWebHostEnvironment hostingEnvironment, IBlogService blogService)
        {
            _competitonService = competitonService;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _blogService = blogService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        //Anasayfa
        public async Task<IActionResult> AllCompetitions(int page = 1)
        {
            var allCompetitions = await _competitonService.GetAllAsync();
            var totalCompetitions = allCompetitions.Count();
            var totalPages = (int)Math.Ceiling(totalCompetitions / (double)CompetitionsPerPage);

            var competitions = allCompetitions
                .Skip((page - 1) * CompetitionsPerPage)
                .Take(CompetitionsPerPage)
                .ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.Competitions = competitions;

            return View();
        }

        // Benim yarışmalarım
        public async Task<IActionResult> MyAllCompetitions()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var myvalue = _competitonService.Where(x => x.CustomerId == user.Id).ToList();
            myvalue.Reverse();
            // var value2 = await _competitonService.GetAllAsync();
            return View(myvalue);
        }

        // Bir tane yarışmam 
        //[HttpGet]
        //public async Task<IActionResult> Get(int id)
        //{
        //    Competition compatition = await _competitonService.GetByIdAsync(id);
        //    return View(compatition);
        //}

        // Yarışma oluştur.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CompetitonCreateDto createDto, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    var uploadsFolder = Path.Combine(webRootPath, "UserFiles");

                    // Eğer UserFiles klasörü yoksa oluşturun
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    createDto.FilePath = filePath;
                }

                var value = await _userManager.FindByNameAsync(User.Identity.Name);
                createDto.CustomerId = value.Id;
                _competitonService.CreateCompetition(createDto);

                return RedirectToAction("Index");
            }

            return View(createDto);
        }



    }
}
