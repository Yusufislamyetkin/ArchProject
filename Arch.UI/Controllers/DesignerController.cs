using Arch.BussinessLayer.Abstract;
using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class DesignerController : Controller
    {
        private readonly ICompetitonService _competitonService;
        private readonly IDesignerUserService _designerUserService;
        private readonly UserManager<AppUser> _userManager;

        public DesignerController(ICompetitonService competitonService, UserManager<AppUser> userManager, IDesignerUserService designerUserService)
        {
            _competitonService = competitonService;
            _userManager = userManager;
            _designerUserService = designerUserService;
        }

        [HttpPost]
        public async Task<IActionResult> JoinCompetition(int id)
        {
            var competition = await _competitonService.Where(x => x.Id == id).Include(x => x.DesignerUsers).ThenInclude(x => x.Designer).FirstOrDefaultAsync();
            var designer = await _userManager.FindByNameAsync(User.Identity.Name);

            DesignerUser designerUser = new DesignerUser
            {
                Designer = designer,
                CompetitionId = competition.Id
            };

            if (competition != null && designer != null)
            {
                if (competition.DesignerUsers == null)
                {
                    competition.DesignerUsers = new List<DesignerUser>();
                }

                if (!competition.DesignerUsers.Contains(designerUser))
                {
                    competition.DesignerUsers.Add(designerUser);
                }

                _competitonService.UnitOfWork();
                //await _competitonService.UpdateAsync(competition);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> LeaveCompetition(int id)
        {
            var competition = await _competitonService.Where(x => x.Id == id).Include(x => x.DesignerUsers).ThenInclude(x => x.Designer).FirstOrDefaultAsync();
            var designer = await _userManager.FindByNameAsync(User.Identity.Name);

            if (competition != null && designer != null && competition.DesignerUsers != null)
            {
                var designerUser = competition.DesignerUsers.FirstOrDefault(x => x.DesignerId == designer.Id);
                if (designerUser != null)
                {
                    var deletedValue = await _designerUserService.GetByIdAsync(designerUser.Id);
                    await _designerUserService.RemoveAsync(deletedValue);
                    //var value =  competition.DesignerUsers.Remove(designerUser);
                    //_competitonService.UnitOfWork();
                    //await _competitonService.UpdateAsync(competition);
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
    }
}
