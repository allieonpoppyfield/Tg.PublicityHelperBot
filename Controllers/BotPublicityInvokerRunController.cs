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
            try
            {
                if (update.Message != null && update.Message.ForwardFromChat != null && update.Message.ForwardFromChat.Type == Telegram.Bot.Types.Enums.ChatType.Channel)
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
            }
            catch (Exception ex)
            {
                _ = ex;
            }
            return Ok();
        }
        private async Task ManageForwardedMessage(Update update)
        {
            await _updateService.HandleForwardedMessage(update);
        }

        private async Task ManageCallBack(Update update)
        {
            var callBackData = update.CallbackQuery.Data;

            if (callBackData == CallBackActionNames.MainMenu)
            {
                await _updateService.HandleMainMenulAction(update);
            }
            else if (callBackData == CallBackActionNames.Account)
            {
                await _updateService.HandleAccountMenuAction(update);
            }
            else if (callBackData == CallBackActionNames.AddChannel)
            {
                await _updateService.HandleAddChannelAction(update);
            }
            else if (callBackData.StartsWith(CallBackActionNames.ChannelInfo))
            {
                await _updateService.HandleChannelInfoAction(update);
            }
            else if (callBackData.StartsWith(CallBackActionNames.CreatePost))
            {
                await _updateService.HandleCreatePostAction(update);
            }
            else if (callBackData.StartsWith(CallBackActionNames.Publish))
            {
                await _updateService.HandlePublishAction(update);
            }
            else if (callBackData.StartsWith(CallBackActionNames.DeleteChannel))
            {
                await _updateService.HandleDeleteChannelAction(update);
            }
            else if (callBackData.StartsWith(CallBackActionNames.YesDelete))
            {
                await _updateService.HandleYesDeleteChannelAction(update);
            }
        }

        private async Task ManageTextMessage(Update update)
        {
            string textMessage = update.Message.Text;
            if (textMessage == "/start")
            {
                await _updateService.HandleStartMessage(update);
                return;
            }
            else if (textMessage == KeyboardTitles.MainMenuTitle)
            {
                await _updateService.HandleMainMenuMessage(update);
                return;
            }
            else if (textMessage == KeyboardTitles.AccountTitle)
            {
                await _updateService.HandleAccountMenuMessage(update);
                return;
            }
            else
            {
                await _updateService.HandleTextMessageAction(update);
            }
        }


        [HttpGet]
        public string Post()
        {
            return "Test";
        }
    }
}
