﻿using ZDoneWebApi.Data.DTOs;

namespace ZDoneWebApi.BusinessLogic.Responses
{
    public class ItemResponse : BaseResponse
    {
        /// <summary>
        /// Instance of ItemDto which can be returned to user with answer if it's need. (Not required)
        /// </summary>
        ///

        public ItemDto ItemDto { get; private set; }
        public int id { get; set; }

        /// <summary>
        /// Constructor which initialize ItemDto field.
        /// </summary>
        /// <param name="success">State of operation</param>
        /// <param name="message">Message which will be returned to cliend</param>
        /// <param name="itemDto">Item which will be returned to cliend</param>
        private ItemResponse(bool success, string message, ItemDto itemDto) : base(success, message)
        {
            ItemDto = itemDto;
        }

        /// <summary>
        /// Second constructor if we want to return only ItemDto.
        /// </summary>
        /// <param name="itemDto"></param>
        public ItemResponse(ItemDto itemDto) : this(true, string.Empty, itemDto)
        { }

        /// <summary>
        /// Third constructor if we want to return only message and state, without ItemDto.
        /// </summary>
        /// <param name="state">State of operation</param>
        /// <param name="message">Message which will be returned to cliend</param>
        public ItemResponse(bool state, string message) : this(state, message, null)
        { }

        public ItemResponse(bool state, string message, int id) : this(state, message, null)
        {
            this.id = id;
        }
    }
}