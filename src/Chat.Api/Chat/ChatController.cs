using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Core.Dtos;
using Chat.Core.Features.Chat.CreateRooms;
using Chat.Core.Features.Chat.LoadMessagesByCount;
using Chat.Core.Features.Chat.LoadReceivedMessages;
using Chat.Core.Features.Chat.SendMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Chat
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessageAsync(SendMessageCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(id);
        }

        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoomAsync(CreateRoomCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("load-messages-by-count/{userName}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> LoadMessagesByCounts([FromRoute] string userName,
            [FromQuery] int? numberOfMessages = null)
        {
            var messages = await _mediator.Send(new LoadMessageByCountQuery(userName, numberOfMessages));

            return Ok(messages);
        }


        [HttpGet("received-messages/{userName}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> LoadByMessagesByTimes([FromRoute] string userName,
            [FromQuery] DateTime? dateTime)
        {
            var messages = await _mediator.Send(new LoadReceivedMessagesQuery(userName, dateTime));

            return Ok(messages);
        }
    }
}