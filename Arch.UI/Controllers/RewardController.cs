using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.EntityLayer.Entities;
using Arch.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Arch.UI.Controllers
{
    public class RewardController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IRewardService  _rewardService;

        public RewardController(IFileService fileService, IRewardService rewardService)
        {
            _fileService = fileService;
            _rewardService = rewardService;
        }


        // POST: /Reward/SaveSelections
        [HttpPost]
        public async Task<ActionResult> SaveSelections(List<SelectionViewModel> selections)
        {
            if (selections.Count != 0 )
            {
              var value =  await _rewardService.CreateRewardAddRange(selections);
                if (value)
                {
                    var responseT = new { success = true };
                    return Json(responseT);
                }
                else
                {
                    var responseF = new { success = false };
                    return Json(responseF);
                }
              
            }
            var response = new { success = false };
            return Json(response);
        }
    }
}
