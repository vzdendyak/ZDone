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
    public class ItemBl : IItemBl
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemBl(IItemRepository itemRepository, IMapper mapper)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _itemRepository.ReadAll();

            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(items);

            return dtoItems;
        }

        public async Task<ItemDto> ReadAsync(int id)
        {
            var item = await _itemRepository.Read(id);
            var dtoItem = _mapper.Map<ItemDto>(item);

            return dtoItem;
        }

        public async Task<ItemResponse> CreateAsync(ItemDto item)
        {
            item.CreatedDate = DateTime.Now;
            item.ExpiredDate = null;
            var origItem = _mapper.Map<Item>(item);
            await _itemRepository.Create(origItem);

            var returnItem = _mapper.Map<ItemDto>(_itemRepository.GetLastItemStored());
            return new ItemResponse(returnItem);
        }

        public async Task<ItemResponse> UpdateAsync(ItemDto item)
        {
            var existingItem = await _itemRepository.Read(item.Id);
            // Get role
            var newItem = _mapper.Map<Item>(item);
            await _itemRepository.Update(newItem);
            return new ItemResponse(true, "Updated successfully!");
        }

        public async Task<ItemResponse> DeleteAsync(int id)
        {
            var realItem = await _itemRepository.Read(id);

            if (realItem == null) throw new NotImplementedException();
            await _itemRepository.Delete(id);
            return new ItemResponse(true, "Deleted successfully");
        }
    }
}