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

            builder = new KeyboardBuilder();
            builder.AddButton("Новая комната", "NewRoom", KeyboardButtonColor.Positive);
            builder.AddButton("Подключиться", "ConnectToRoom", KeyboardButtonColor.Positive);
            builder.SetOneTime();
            Main = builder.Build();
        }
    }
}
