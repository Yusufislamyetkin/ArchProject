using Arch.BussinessLayer.Abstract;
using Arch.BussinessLayer.Dtos;
using Arch.DataAccessLayer.Abstract;
using Arch.EntityLayer.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Arch.BussinessLayer.Concrete
{
    public class RewardService : Service<Reward>, IRewardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Reward> _repository;
        private readonly IService<Reward> _service;
        public RewardService(IRepository<Reward> repository, IUnitOfWork unitOfWork, IMapper mapper, IService<Reward> service) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _service = service;
        }

        public async Task<bool> CreateRewardAddRange(List<SelectionViewModel>  selections)
        {
            var  rewards = _mapper.Map<List<Reward>>(selections);
            foreach (Reward reward in rewards)
            {
                var checkStatus = await _repository.Where(x => x.DesignerId == reward.DesignerId && x.CompetitionId == reward.CompetitionId).FirstOrDefaultAsync();

                if (checkStatus != null)
                {
                     _repository.Remove(checkStatus);
                 

                }
                await _repository.AddAsync(reward);
                await _unitOfWork.CommitAsync();

            }
            return true;
        }

    }
}

