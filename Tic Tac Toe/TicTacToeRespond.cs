using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Keyboard;

namespace TicTacToeGame
{
    struct TicTacToeRespond
    {
        string Message;
        MessageKeyboard Keyboard;

        public TicTacToeRespond(string message, MessageKeyboard keyboard)
        {
            Message = message;
            Keyboard = keyboard;
        }
    }
}
