using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.DataAccessLayer.Abstract;
using Arch.EntityLayer.Entities;
using AutoMapper;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.BussinessLayer.Concrete
{
    public class CompetitonService : Service<Competition>, ICompetitonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Competition> _repository;
        public ICollection<AppUser> Contestants { get; set; } // Yarışmacılar koleksiyonu
        public CompetitonService(IRepository<Competition> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            Contestants = new List<AppUser>();
        }

        public void CreateCompetition(CompetitonCreateDto competitionCreateDto)
        {
            // Competition oluşturma işlemlerini gerçekleştirin
            // Örneğin, repository üzerinden kaydetme işlemi yapabilirsiniz
            var competition = _mapper.Map<Competition>(competitionCreateDto);
            _repository.AddAsync(competition);
            _unitOfWork.Commit();
        }

        public void UnitOfWork()
        {

            _unitOfWork.Commit();
        }

        public void AddContestant(AppUser contestant)
        {
            // İlgili yarışmacının yarışmaya zaten katılıp katılmadığını kontrol edin
            if (!Contestants.Any(c => c.Id == contestant.Id))
            {
                // Yarışmacıyı yarışmaya ekleyin
                Contestants.Add(contestant);
            }
        }



    }
}

