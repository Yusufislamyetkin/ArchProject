using Arch.BussinessLayer.Abstract;
using Arch.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.UI.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;
        private readonly UserManager<AppUser> _userManager;


        public FileController(IWebHostEnvironment env, IFileService fileService, UserManager<AppUser> userManager)
        {
            _env = env;
            _fileService = fileService;
            _userManager = userManager;
        }
   

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files, int competitionId)
        {
            List<ProjectFilePath> projectFileList = new List<ProjectFilePath>();

            foreach (var file in files)
            {
                
                if (file.Length > 0)
                {
                    var webRootPath = _env.WebRootPath;
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

                    var designer = await _userManager.FindByNameAsync(User.Identity.Name);

                    ProjectFilePath projectFilePath = new ProjectFilePath();

                    projectFilePath.CompetitionId = competitionId;
                    projectFilePath.Address = "/" + Path.Combine("UserFiles", fileName).Replace("\\", "/");
                    projectFilePath.DesignerId = designer.Id;
                    projectFilePath.Type = 1;
                 
                    var projectFile = await _fileService.CreateFile(projectFilePath);
                    projectFileList.Add(projectFile);
                }
            }

            return Json(projectFileList);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string fileId)
        {
            var entity = await _fileService.GetByIdAsync(int.Parse(fileId));
            if (entity != null)
            {
                var filePath = Path.Combine(_env.WebRootPath, entity.Address.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                await _fileService.RemoveAsync(entity);
            }

            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> Get(int competitionId)
        {
            var designer = await _userManager.FindByNameAsync(User.Identity.Name);
            var uploadedDatas =  _fileService.Where(x => x.DesignerId == designer.Id && x.Type == 1).ToList();       
            return Json(uploadedDatas);
        }


    }


}




