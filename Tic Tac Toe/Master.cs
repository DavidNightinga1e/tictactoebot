using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe
{
    class Master
    {

        static VkApiShell Shell;
        static Dictionary<long, Game> Tables;
        static Dictionary<string, long> Awaiting;

        static void Main()
        {
            Shell = new VkApiShell("a6c333fd9867e969d04c51b2d22936131341bf43c6fa7d01ef7997da677f62ce30deb2afe351057dc2f1a")
            {
                LongPollWait = 25
            };

            Awaiting = new Dictionary<string, long>();
            Tables = new Dictionary<long, Game>();

            while (true)
            {
                // Получение новых сообщений
                var respond = Shell.GetNewMessages();
                
                // Обработка каждого сообщения
                foreach (var message in respond)
                {
                    // Если пользователь в игре
                    if (Tables.ContainsKey(message.Id))
                    {
                        var table = Tables[message.Id];
                        // Обработка нажатия пользователем кнопки
                        var res = table.ButtonPush(message.Id, message.Payload);

                        switch (res)
                        {
                            case ButtonPushResult.Set:
                                Shell.SendKeyboard(table.Player1, "Клавиатура обновлена", table.Field.Keyboard);
                                Shell.SendKeyboard(table.Player2, "Клавиатура обновлена", table.Field.Keyboard);
                                break;

                            case ButtonPushResult.NotYourTurn:
                                Shell.SendMessage(message.Id, "Ход оппонента, ожидайте");
                                break;

                            case ButtonPushResult.Taken:
                                Shell.SendMessage(message.Id, "Сюда нельзя");
                                break;
                        }

                        if (table.IsEnded)
                        {
                            if (table.Winner != 0)
                            {
                                Shell.SendMessage(table.Winner, "Поздравляю, Вы выиграли!");
                                Shell.SendMessage(table.Lost, "К сожалению, Вы проиграли!");
                            }
                            else
                            {
                                Shell.SendMessage(table.Player1, "Ничья");
                                Shell.SendMessage(table.Player2, "Ничья");
                            }

                            Tables.Remove(table.Player1);
                            Tables.Remove(table.Player2);
                        }
                    }
                    else
                    {
                        // Если сообщение пользователя есть в списке ключей
                        if (Awaiting.ContainsKey(message.Text))
                        {
                            // Проверка что пользователь не ввел повторно тот же пароль
                            if (Awaiting[message.Text] != message.Id)
                            {
                                // Создание новой игры
                                CreateNewGame(Awaiting[message.Text], message.Id);
                                Awaiting.Remove(message.Text);
                            }
                            else
                            {
                                Shell.SendMessage(message.Id, "Вы уже ввели этот ключ");
                            }
                        }
                        else
                        {
                            Awaiting.Add(message.Text, message.Id);
                            Shell.SendMessage(message.Id, $"Вы добавлены в список ожидания с ключом {message.Text}, сообщите его своему другу для начала матча");
                        }
                    }
                }
            }


            void CreateNewGame(long player1, long player2)
            {
                var newgame = new Game(player1, player2);
                Tables.Add(player1, newgame);
                Tables.Add(player2, newgame);

                Shell.SendKeyboard(player1, "Вы играете X, Ваш ход!", newgame.Field.Keyboard);
                Shell.SendKeyboard(player2, "Вы играете О, ходит оппонент", newgame.Field.Keyboard);
            }
        }
    }
}
