using Arch.BussinessLayer.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class CompetitonController : Controller
    {
        private readonly ICompetitonService _competitonService;
        readonly UserManager<AppUser> _userManager;
        public CompetitonController(ICompetitonService competitonService, UserManager<AppUser> userManager)
        {
            _competitonService = competitonService;
            _userManager = userManager;
        }




        public IActionResult AllCompetitons()
        {
            return View();
        }

        public async Task<IActionResult> MyAllCompetitons()
        {
            var value = await _userManager.FindByNameAsync(User.Identity.Name);
          var myvalue =    _competitonService.Where(x => x.CustomerId == value.Id).FirstOrDefault();
           var value2 = await _competitonService.GetAllAsync();
            return View();
        }

        public IActionResult CreateCompetitons()
        {
            return View();
        }
        public IActionResult EditCompetitons()
        {
            return View();
        }
        public IActionResult DeleteCompetitons()
        {
            return View();
        }
    }
}
