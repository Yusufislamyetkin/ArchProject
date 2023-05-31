using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ICompetitonService _competitonService;
        private readonly UserManager<AppUser> _userManager;
        private const int CompetitionsPerPage = 12;

        public CompetitionController(ICompetitonService competitonService, UserManager<AppUser> userManager)
        {
            _competitonService = competitonService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var value = User.Identity.Name;
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
        public IActionResult MyAllCompetitions()
        {
            //var value = await _userManager.FindByNameAsync(User.Identity.Name);
            var myvalue = _competitonService.Where(x => x.CustomerId == "37d28aa8-3182-4e7a-9441-5a417d3030e4").ToList();
            // var value2 = await _competitonService.GetAllAsync();
            return View(myvalue);
        }

        // Bir tane yarışmam 
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            Competition compatition = await _competitonService.GetByIdAsync(id);
            return View(compatition);
        }

        // Yarışma oluştur.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CompetitonCreateDto createDto)
        {
            //var value = await _userManager.FindByNameAsync(User.Identity.Name);
            createDto.CustomerId = "37d28aa8-3182-4e7a-9441-5a417d3030e4";
            _competitonService.CreateCompetition(createDto);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomerBlogPost(int id, BlogPost model)
        {
            if (ModelState.IsValid)
            {
                var competition = await _competitonService.GetByIdAsync(id);
                var customer = await _userManager.FindByNameAsync(User.Identity.Name);

                if (competition != null && customer != null && competition.CustomerId == customer.Id)
                {
                    model.AuthorId = customer.Id;
                    competition.BlogPosts.Add(model);
                    await _competitonService.UpdateAsync(competition);

                    return RedirectToAction("CompetitionDetails", new { id });
                }
            }

            return View("_CustomerBlogPostForm", model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateDesignerBlogPost(int id, BlogPost model)
        {
            if (ModelState.IsValid)
            {
                var competition = await _competitonService.GetByIdAsync(id);
                var designer = await _userManager.FindByNameAsync(User.Identity.Name);

                if (competition != null && designer != null && competition.Designers.Any(d => d.Id == designer.Id))
                {
                    model.AuthorId = designer.Id;
                    competition.BlogPosts.Add(model);
                    await _competitonService.UpdateAsync(competition);

                    return RedirectToAction("CompetitionDetails", new { id });
                }
            }

            return View("_DesignerBlogPostForm", model);
        }

    }
}
