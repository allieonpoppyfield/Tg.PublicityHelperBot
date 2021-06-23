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
        private readonly IUpdateService _updateService;
        private readonly IChatCollectionService _chatCollectionService;

        public BotPublicityInvokerRunController(IUpdateService updateService, IChatCollectionService chatCollectionService)
        {
            _updateService = updateService;
            _chatCollectionService = chatCollectionService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            string messageText = update.Message.Text;
            var me = update.Message.From;
            switch (messageText)
            {
                //редактирование поста
                case MenuItemNames.EditPost:
                    await EditPostAction(update);
                    break;

                //главное меню
                case MenuItemNames.MainMenuStart:
                case MenuItemNames.MainMenuVisible:
                default:
                    await MainMenuAction(update);
                    break;
            }


            //StateEdin curSt = _stateService.List.FirstOrDefault(x => x.ChatId == update.Message.Chat.Id);
            //if (curSt == null)
            //{
            //    curSt = new StateEdin(update.Message.Chat.Id);
            //    _stateService.List.Add(curSt);
            //}

            //var txt = update.Message.Text;
            //if (txt.ToLower().Contains("запомни это"))
            //{
            //    curSt.CurrentMessage = txt.Replace("запомни это", "");
            //}
            //else if (txt.ToLower() == "повтори")
            //{
            //    await _updateService.SendMessage(curSt.CurrentMessage == "" ? "нет сообщения" : curSt.CurrentMessage, update.Message.Chat.Id);
            //}

            return Ok();
        }

        [HttpGet]
        public string Post()
        {
            return "Test";
        }


        private async Task MainMenuAction(Update update)
        {
            await _updateService.SendTextMessageAssync(update.Message.Chat.Id, Messages.MainMenuMessage, MenuItemMarkups.MainMenuItems);
        }

        private async Task EditPostAction(Update update)
        {
            await _updateService.SendTextMessageAssync(update.Message.Chat.Id, Messages.EditPostMessage, MenuItemMarkups.EditPostItems);
        }
    }
}
