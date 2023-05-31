using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.DataAccessLayer.Abstract;
using Arch.EntityLayer.Entities;
using AutoMapper;

namespace Arch.BussinessLayer.Concrete
{
    public class CompetitonService : Service<Competition>, ICompetitonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Competition> _repository;
        public CompetitonService(IRepository<Competition> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public void CreateCompetition(CompetitonCreateDto competitionCreateDto)
        {
            // Competition oluşturma işlemlerini gerçekleştirin
            // Örneğin, repository üzerinden kaydetme işlemi yapabilirsiniz
            var competition = _mapper.Map<Competition>(competitionCreateDto);
            _repository.AddAsync(competition);
            _unitOfWork.Commit();
        }




    }
}

