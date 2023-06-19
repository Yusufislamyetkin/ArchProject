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

        [HttpPost]
        public IActionResult SaveSelections([FromBody] List<SelectionModel> selections)
        {
            // selections verisini işleme
            // ... burada yapılacak işlemler ...

            return Ok(); // İşlem başarılı olduğunda 200 OK yanıtı döndürülür
        }
    }
}
