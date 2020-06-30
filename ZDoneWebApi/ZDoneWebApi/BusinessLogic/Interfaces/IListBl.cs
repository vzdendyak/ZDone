﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.BusinessLogic.Interfaces
{
    public interface IListBl
    {
        Task<IEnumerable<ListDto>> GetAllAsync();

        Task<IEnumerable<ItemDto>> GetItemsByListId(int id);

        Task<IEnumerable<ItemDto>> GetDoneItemsByListId(int id);

        Task<IEnumerable<ItemDto>> GetUndoneItemsByListId(int id);

        Task<ListDto> ReadAsync(int id, string userId);

        Task<ItemResponse> CreateAsync(ListDto list);

        Task<ItemResponse> UpdateAsync(ListDto list);

        Task<ItemResponse> DeleteAsync(int id);
    }
}