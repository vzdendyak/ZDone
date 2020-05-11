using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> ReadAll();

        Task<Item> Read(int id);

        Task Create(Item item);

        Task Update(Item item);

        Task Delete(int id);
    }
}