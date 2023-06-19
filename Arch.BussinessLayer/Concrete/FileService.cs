using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.DataAccessLayer.Abstract;
using Arch.EntityLayer.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Arch.BussinessLayer.Concrete
{
    public class FileService : Service<ProjectFilePath>, IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<ProjectFilePath> _repository;
        public FileService(IRepository<ProjectFilePath> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ProjectFilePath> CreateFile(ProjectFilePath projectFilePath)
        {
            ;
            await _repository.AddAsync(projectFilePath);
            await _unitOfWork.CommitAsync();
            return projectFilePath;
        }

        // Yarışma bilgilerinde yer alan dosyalar için
        public async Task<List<ProjectFilePath>> GetByCompIdForView(int Id)
        {
            return await _repository.Where(x => x.CompetitionId == Id && x.Type == 0).ToListAsync();
        }

        // Yarışmaya katılan tasarımcıların yüklediği dosyalara erişmek için
        public async Task<List<ProjectFilePath>> GetByCompIdForTable(int Id)
        {
            return await _repository.Where(x => x.CompetitionId == Id && x.Type == 1).Include(x=> x.Designer).ToListAsync();
        }





    }
}

