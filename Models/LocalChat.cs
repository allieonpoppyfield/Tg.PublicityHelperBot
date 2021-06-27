using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tg.PublicityHelperBot.Models.UserStates;
using Tg.PublicityHelperBot.Services;

namespace Tg.PublicityHelperBot.Models
{
    public class LocalChat
    {
        public long ChatId { get; set; }
        public ChatState State { get; private set; }
        public IUpdateService updateService { get; set; }

        public async Task SetState(UserStatesEnum state)
        {
            switch (state)
            {
                case UserStatesEnum.MainMenu:
                    State = new MainMenuState(updateService, this);
                    await State.StateSelected(ChatId);
                    break;
                case UserStatesEnum.EditPost:
                    State = new EditPostState(updateService, this);
                    await State.StateSelected(ChatId);
                    break;
                case UserStatesEnum.AddChannel:
                    State = new AddChannelState(updateService, this);
                    await State.StateSelected(ChatId);
                    break;
            }
        }
    }
}
