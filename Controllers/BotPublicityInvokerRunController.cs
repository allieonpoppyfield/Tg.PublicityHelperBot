using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Tg.PublicityHelperBot.Models;
using Tg.PublicityHelperBot.Services;

namespace Tg.PublicityHelperBot.Controllers
{
    [Route("api/[controller]")]
    public class BotPublicityInvokerRunController : Controller
    {
        private readonly IChatCollectionService _chatCollectionService;
        private readonly IUpdateService _updateService;

        public BotPublicityInvokerRunController(IChatCollectionService chatCollectionService, IUpdateService updateService)
        {
            _chatCollectionService = chatCollectionService;
            _updateService = updateService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Message == null)
                return Ok();


            var chatID = update.Message.Chat.Id;
            var currentChat = _chatCollectionService.LocalChatsList.FirstOrDefault(x => x.ChatId == chatID);

            if (currentChat == null)
            {
                currentChat = new LocalChat();
                currentChat.ChatId = chatID;
                currentChat.updateService = _updateService;
                _chatCollectionService.LocalChatsList.Add(currentChat);
                await currentChat.SetState(Models.UserStates.UserStatesEnum.MainMenu);
            }

            else
            {
                currentChat.updateService = _updateService;
                await currentChat.State.ProcessUpdate(update);
            }


            return Ok();
        }

        [HttpGet]
        public string Post()
        {
            return "Test";
        }
    }
}
