using Arch.BussinessLayer.Abstract;
using Arch.EntityLayer.Entities;
using Arch.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Arch.UI.Controllers
{
    public class RewardController : Controller
    {
        private readonly IFileService _fileService;

        public RewardController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(int id)
        {
            id = 12;
            List<ProjectFilePath> filesForTable = await _fileService.GetByCompIdForTable(id);

            var designers = filesForTable.Select(file => file.Designer).Distinct().ToList();
            ViewBag.Designers = designers;

            return View(filesForTable);
        }

        public async Task<IActionResult> Index2(int id)
        {
            id = 12;
            List<ProjectFilePath> filesForTable = await _fileService.GetByCompIdForTable(id);

            var designers = filesForTable.Select(file => file.Designer).Distinct().ToList();
            ViewBag.Designers = designers;

            return View(filesForTable);
        }

        // POST: /Reward/SaveSelections
        [HttpPost]
        public ActionResult SaveSelections(List<SelectionViewModel> selections)
        {
            var response = new { success = true };
            return Json(response);
            // selections listesini işle ve yarışma statülerini kaydet

            foreach (var selection in selections)
            {
                string designerId = selection.DesignerId;
                int selectedOption = selection.SelectedOption;

                // Yarışma statüsünü kaydetmek için ilgili işlemleri yapın
                // Örneğin, veritabanına kaydedebilirsiniz
            }

            // Başarılı bir şekilde kaydedildikten sonra bir sayfaya yönlendirin veya başka bir işlem yapın
            return RedirectToAction("Index");
        }
    }
}
