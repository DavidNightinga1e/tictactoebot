using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Keyboard;
using VkNet.Enums.SafetyEnums;

namespace TicTacToeGame
{
    class TicTacToeKeyboards
    {
        public MessageKeyboard Empty { private set; get; }
        public MessageKeyboard Main { private set; get; }

        public TicTacToeKeyboards()
        {
            var builder = new KeyboardBuilder();
            builder.SetOneTime();
            Empty = builder.Build();
            
            // todo - переделать под KeyboardBuilder
            MessageKeyboardButton buttonNewRoom = new MessageKeyboardButton
            {
                Action = new MessageKeyboardButtonAction
                {
                    Label = "Новая комната",
                    Payload = "{\"button\":\"NewRoom\"}"
                },
                Color = KeyboardButtonColor.Positive
            };

            MessageKeyboardButton buttonConnectToRoom = new MessageKeyboardButton
            {
                Action = new MessageKeyboardButtonAction
                {
                    Label = "Подключиться",
                    Payload = "{\"button\":\"ConnectToRoom\"}"
                },
                Color = KeyboardButtonColor.Positive
            };

            Main = new MessageKeyboard
            {
                OneTime = true,
                Buttons = new[] { new[] { buttonNewRoom, buttonConnectToRoom } }
            };
        }
    }
}
