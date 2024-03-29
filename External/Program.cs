﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame;

namespace External
{
    class Program
    {
        private static void Main()
        {
            var TicTacToeApi = new TicTacToe();
            var Shell = new VkApiShell("a6c333fd9867e969d04c51b2d22936131341bf43c6fa7d01ef7997da677f62ce30deb2afe351057dc2f1a")
            {
                LongPollWait = 25
            };

            while (true)
            {
                // Получение новых сообщений
                var respond = Shell.GetNewMessages();

                // Обработка каждого сообщения
                foreach (var message in respond)
                {
                    var tttresponds = TicTacToeApi.Respond(message.Id, message.Text, message.Payload);

                    foreach (var tttrespond in tttresponds)
                    {
                        Shell.SendKeyboard(tttrespond.Id, tttrespond.Message, tttrespond.Keyboard);
                    }
                }
            }
        }
    }
}
