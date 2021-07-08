using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Tg.PublicityHelperBot.Models.Database;
using Tg.PublicityHelperBot.Services.Bot;
using Tg.PublicityHelperBot.Static;

namespace Tg.PublicityHelperBot.Controllers
{
    [Route("api/[controller]")]
    public class BotPublicityInvokerRunController : Controller
    {
        private readonly IUpdateService _updateService;
        private readonly TgDatabaseContext context;

        public BotPublicityInvokerRunController(IUpdateService updateService, TgDatabaseContext ctx)
        {
            _updateService = updateService;
            context = ctx;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if(update.Message != null && update.Message.ForwardFromChat != null && update.Message.ForwardFromChat.Type == Telegram.Bot.Types.Enums.ChatType.Channel)
            {
                await ManageForwardedMessage(update);
            }

            else if (update.Message != null && update.CallbackQuery == null)
            {
                await ManageTextMessage(update);
                return Ok();
            }
            else if (update.CallbackQuery != null)
            {
                await ManageCallBack(update);
                return Ok();
            }
            return Ok();
        }

        private async Task ManageForwardedMessage(Update update)
        {
            await _updateService.HandleForwardedMessage(update);
        }

        private async Task ManageCallBack(Update update)
        {
            if (update.CallbackQuery.Data == CallBackActionNames.MainMenu)
            {
                await _updateService.HandleMainMenulAction(update);
            }

            if (update.CallbackQuery.Data == CallBackActionNames.AddChannel)
            {
                await _updateService.HandleAddChannelAction(update);
            }
        }

        private async Task ManageTextMessage(Update update)
        {
            string textMessage = update.Message.Text;
            if (textMessage == "/start")
            {
                await _updateService.HandleStartMessage(update);
            }
            else if (textMessage == KeyboardTitles.MainMenuTitle)
            {
                await _updateService.HandleMainMenuMessage(update);
            }
            else if (textMessage == KeyboardTitles.AccountTitle)
            {
                await _updateService.HandleAccountMenuMessage(update);
            }
        }


        [HttpGet]
        public string Post()
        {
            return "Test";
        }
    }
}
