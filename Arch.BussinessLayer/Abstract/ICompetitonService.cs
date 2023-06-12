using Arch.BussinessLayer.Dtos;
using Arch.EntityLayer.Entities;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.BussinessLayer.Abstract
{
    public interface ICompetitonService : IService<Competition>
    {
        Task<Competition> CreateCompetition(CompetitonCreateDto competitionCreateDto);
        void AddContestant(AppUser contestant);
        Task UnitOfWorkAsync();
    }
}
