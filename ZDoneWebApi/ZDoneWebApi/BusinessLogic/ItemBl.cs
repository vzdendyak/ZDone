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
        private readonly IListRepository _listRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IUserBl _userBl;
        private readonly IMapper _mapper;

        public ItemBl(IItemRepository itemRepository, IListRepository listRepository, IUserBl userBl, IFolderRepository folderRepository, IMapper mapper)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
            _listRepository = listRepository;
            _folderRepository = folderRepository;
            _userBl = userBl;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _itemRepository.GetAll();

            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(items);

            return dtoItems;
        }

        public async Task<IEnumerable<ItemDto>> GetDeletedItems()
        {
            var items = await _itemRepository.GetDeletedItems();
            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(items);

            return dtoItems;
        }

        public async Task<IEnumerable<ItemDto>> GetAllByProject(int id, string userId)
        {
            if (!(await _userBl.IsHaveProjectPermission(userId, id)))
            {
                return null;
            }
            List<Item> items = new List<Item>();
            var folders = await this._folderRepository.GetByProjectId(id);
            foreach (var f in folders)
            {
                var lists = await _folderRepository.GetAllLists(f.Id);
                foreach (var l in lists)
                {
                    var items1 = await _listRepository.GetItemsByListId(l.Id);
                    items.AddRange(items1);
                }
            }
            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(items);

            return dtoItems;
        }

        public async Task<IEnumerable<ItemDto>> GetDateItems(string date)
        {
            IEnumerable<Item> items = null;
            if (date.ToLower() == "today")
            {
                items = await _itemRepository.GetTodayItems();
            }
            else if (date.ToLower() == "week")
            {
                items = await _itemRepository.GetWeekItems();
            }

            if (items != null)
            {
                IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(items);
                return dtoItems;
            }

            return null;
        }

        public async Task<IEnumerable<ItemDto>> GetCompletedItems()
        {
            var items = await _itemRepository.GetCompletedItems();

            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(items);
            return dtoItems;
        }

        public async Task<IEnumerable<ItemDto>> GetUnlistedItems()
        {
            var items = await _itemRepository.GetUnlistedItems();

            IEnumerable<ItemDto> dtoItems = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDto>>(items);
            return dtoItems;
        }

        public async Task<ItemDto> ReadAsync(int id, string userId, int projectId)
        {
            if (!(await _userBl.IsHaveAccesToItem(id, userId)))
            {
                return null;
            }

            var item = await _itemRepository.Read(id);

            var dtoItem = _mapper.Map<ItemDto>(item);

            return dtoItem;
        }

        public async Task<ItemResponse> CreateAsync(ItemDto item, string userId)
        {
            if (item.ListId == null || item.ListId == 0)
            {
                item.ListId = await _userBl.GetBasicListIdId(userId);
            }
            item.CreatedDate = DateTime.Now;
            item.ExpiredDate = null;
            var origItem = _mapper.Map<Item>(item);
            await _itemRepository.Create(origItem);

            var returnItem = _mapper.Map<ItemDto>(_itemRepository.GetLastItemStored());
            return new ItemResponse(returnItem);
        }

        public async Task<ItemResponse> UpdateAsync(ItemDto item)
        {
            //var existingItem = await _itemRepository.Read(item.Id);
            // Get role
            if (String.IsNullOrWhiteSpace(item.Description))
            {
                item.Description = null;
            }
            var newItem = _mapper.Map<Item>(item);
            await _itemRepository.Update(newItem);
            return new ItemResponse(true, "Updated successfully!");
        }

        public async Task<ItemResponse> DeleteAsync(int id)
        {
            var realItem = await _itemRepository.Read(id);

            if (realItem == null) throw new NotImplementedException();
            realItem.IsDeleted = !realItem.IsDeleted;
            await _itemRepository.Update(realItem);
            return new ItemResponse(true, "(soft)Deleted successfully");
        }
    }
}