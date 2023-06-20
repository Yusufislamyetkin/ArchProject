using Arch.BussinessLayer.Dtos;
using Arch.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arch.BussinessLayer.Abstract
{
    public interface IRewardService : IService<Reward>
    {
        Task<bool> CreateRewardAddRange(List<SelectionViewModel> selections);

    }
}
