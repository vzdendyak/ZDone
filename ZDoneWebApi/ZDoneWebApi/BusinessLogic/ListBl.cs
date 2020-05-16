using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ZDoneWebApi.BusinessLogic.Interfaces;
using ZDoneWebApi.BusinessLogic.Responses;
using ZDoneWebApi.Data.DTOs;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories.Interfaces;

namespace ZDoneWebApi.BusinessLogic
{
    public class ListBl : IListBl
    {
        private readonly IListRepository _listRepository;
        private readonly IMapper _mapper;

        public ListBl(IListRepository listRepository, IMapper mapper)
        {
            _mapper = mapper;
            _listRepository = listRepository;
        }

        public async Task<IEnumerable<ListDto>> GetAllAsync()
        {
            var lists = await _listRepository.ReadAll();

            IEnumerable<ListDto> dtoItems = _mapper.Map<IEnumerable<List>, IEnumerable<ListDto>>(lists);

            return dtoItems;
        }

        public async Task<IEnumerable<ItemDto>> GetItemsByListId(int id)
        {
            var lists = await _listRepository.GetItemsByListId(id);

            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(lists);

            return dtoItems;
        }

        public async Task<IEnumerable<ItemDto>> GetDoneItemsByListId(int id)
        {
            var lists = await _listRepository.GetDoneItemsByListId(id);

            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(lists);

            return dtoItems;
        }

        public async Task<IEnumerable<ItemDto>> GetUndoneItemsByListId(int id)
        {
            var lists = await _listRepository.GetUnDoneItemsByListId(id);

            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(lists);

            return dtoItems;
        }

        public async Task<ListDto> ReadAsync(int id)
        {
            var list = await _listRepository.Read(id);
            var dtoItem = _mapper.Map<ListDto>(list);
            var item = _listRepository.GetLastItemStored();
            return dtoItem;
        }

        public async Task<ItemResponse> CreateAsync(ListDto list)
        {
            var origList = _mapper.Map<List>(list);

            await _listRepository.Create(origList);

            var returnList = _mapper.Map<ListDto>(_listRepository.GetLastItemStored());
            return new ItemResponse(true, "Created", returnList.Id);
        }

        public async Task<ItemResponse> UpdateAsync(ListDto list)
        {
            var existingList = await _listRepository.Read(list.Id);
            // Get role
            var newList = _mapper.Map<List>(list);
            await _listRepository.Update(newList);
            return new ItemResponse(true, "Updated successfully!");
        }

        public async Task<ItemResponse> DeleteAsync(int id)
        {
            var realList = await _listRepository.Read(id);

            if (realList == null) throw new NotImplementedException();
            await _listRepository.Delete(id);
            return new ItemResponse(true, "Deleted successfully");
        }
    }
}