using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Tg.PublicityHelperBot.Services.Bot;
using Tg.PublicityHelperBot.Static;

namespace Tg.PublicityHelperBot.Controllers
{
    [Route("api/[controller]")]
    public class BotPublicityInvokerRunController : Controller
    {
        private readonly IUpdateService _updateService;

        public BotPublicityInvokerRunController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Message != null && update.CallbackQuery == null)
            {
               await ManageTextMessage(update);
            }
            if(update.CallbackQuery != null)
            {
                await ManageCallBack(update);
            }

            return Ok();
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
            else if(textMessage == KeyboardTitles.MainMenuTitle)
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
