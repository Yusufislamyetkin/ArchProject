using Arch.BussinessLayer.Abstract;
using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class DesignerController : Controller
    {
        private readonly ICompetitonService _competitonService;
        private readonly UserManager<AppUser> _userManager;

        public DesignerController(ICompetitonService competitonService, UserManager<AppUser> userManager)
        {
            _competitonService = competitonService;
            _userManager = userManager;
        }

        public async Task<IActionResult> JoinCompetition(int id)
        {
            var competition = await _competitonService.GetByIdAsync(id);
            var designer = await _userManager.FindByNameAsync(User.Identity.Name);

            if (competition != null && designer != null)
            {
                competition.Designers.Add(designer);
                await _competitonService.UpdateAsync(competition);
                return RedirectToAction("JoinCompetitionSuccess");
            }

            return RedirectToAction("JoinCompetitionFailed");
        }

        public async Task<IActionResult> LeaveCompetition(int id)
        {
            var competition = await _competitonService.GetByIdAsync(id);
            var designer = await _userManager.FindByNameAsync(User.Identity.Name);

            if (competition != null && designer != null)
            {
                competition.Designers.Remove(designer);
                await _competitonService.UpdateAsync(competition);
                return RedirectToAction("LeaveCompetitionSuccess");
            }

            return RedirectToAction("LeaveCompetitionFailed");
        }
    }
}
