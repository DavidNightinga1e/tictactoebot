using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace External
{
    class VkApiShell
    {
        public VkApi VkApi { get; private set; }
        public int LongPollWait { set; get; }

        public VkApiShell(string token)
        {
            VkApi = new VkApi();
            VkApi.Authorize(new ApiAuthParams
            {
                AccessToken = token
            });
        }

        public void SendMessage(long id, string message)
        {
            VkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = (int)DateTime.Now.Ticks,
                UserId = id,
                Message = message
            });
        }

        public void SendKeyboard(long id, string message, MessageKeyboard keyboard)
        {
            VkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = (int)DateTime.Now.Ticks,
                UserId = id,
                Message = message,
                Keyboard = keyboard
            });
        }

        public BotsLongPollHistoryResponse SendLongPollRequest()
        {
            var LongPollServerResponse = VkApi.Groups.GetLongPollServer(180715078);

            var response = VkApi.Groups.GetBotsLongPollHistory(
                new BotsLongPollHistoryParams
                {
                    Server = LongPollServerResponse.Server,
                    Key = LongPollServerResponse.Key,
                    Ts = LongPollServerResponse.Ts,
                    Wait = LongPollWait
                });

            return response;
        }

        public UserMessage[] GetNewMessages()
        {
            var response = SendLongPollRequest();
            var updates = response.Updates.ToArray();

            List<UserMessage> messages = new List<UserMessage>();
            foreach (var update in updates)
            {
                messages.Add(new UserMessage(update.Message.FromId.Value, 
                                                update.Message.Text, 
                                                update.Message.Payload));
            }
            return messages.ToArray();
        }
    }
}
