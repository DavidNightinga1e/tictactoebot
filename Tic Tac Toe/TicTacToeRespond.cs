using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Keyboard;

namespace TicTacToeGame
{
    public struct TicTacToeRespond
    {
        public long Id { private set; get; }
        public string Message { private set; get; }
        public MessageKeyboard Keyboard { private set; get; }

        public TicTacToeRespond(long id, string message, MessageKeyboard keyboard)
        {
            Id = id;
            Message = message;
            Keyboard = keyboard;
        }
    }
}
