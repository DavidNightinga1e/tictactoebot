using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class TicTacToe
    {
        private static void Main() { }

        enum DialogStatus
        {
            Menu,
            WaitingForNewRoomId,
            WaitingForExisitingRoomId,
            InGame,
            Ended
        };

        Dictionary<long, DialogStatus> Dialogs;
        Dictionary<uint, long> RoomIdToPlayer1;
        Dictionary<long, Game> Tables;

        TicTacToeKeyboards TicTacToeKeyboards;

        public TicTacToe()
        {
            Dialogs = new Dictionary<long, DialogStatus>();
            RoomIdToPlayer1 = new Dictionary<uint, long>();
            TicTacToeKeyboards = new TicTacToeKeyboards();
            Tables = new Dictionary<long, Game>();
        }

        public TicTacToeRespond[] Respond(long id, string message, string payload)
        {
            if (payload != null)
                payload = payload.Substring(11, payload.Length - 13);
            
            if (Dialogs.ContainsKey(id))
            {
                switch (Dialogs[id])
                {
                    case DialogStatus.Ended:
                        Dialogs[id] = DialogStatus.Menu;
                        return new[]
                        {
                            new TicTacToeRespond(id, "Главное меню", TicTacToeKeyboards.Main)
                        };
                    case DialogStatus.InGame:
                        var result = Tables[id].ButtonPush(id, payload);
                        switch (result)
                        {
                            case ButtonPushResult.NotYourTurn:
                                return new[]
                                {
                                    new TicTacToeRespond(id, "Сейчас ходит оппонент", Tables[id].Field.Keyboard)
                                };
                            case ButtonPushResult.Taken:
                                return new[]
                                {
                                    new TicTacToeRespond(id, "Сюда установить нельзя", Tables[id].Field.Keyboard)
                                };
                            case ButtonPushResult.Set:
                                if (Tables[id].IsEnded)
                                {
                                    var game = Tables[id];
                                    Tables.Remove(game.Player1);
                                    Tables.Remove(game.Player2);

                                    Dialogs[game.Player1] = DialogStatus.Ended;
                                    Dialogs[game.Player2] = DialogStatus.Ended;

                                    return new[]
                                    {
                                        new TicTacToeRespond(game.Winner, "Вы выиграли! Нажмите любую клавишу для возврата в главное меню", game.Field.Keyboard),
                                        new TicTacToeRespond(game.Lost, "Вы проиграли. Нажмите любую клавишу для возврата в главное меню", game.Field.Keyboard),
                                    };
                                }
                                else
                                {
                                    return new[]
                                    {
                                        new TicTacToeRespond(Tables[id].Player1, "Клавиатура обновлена", Tables[id].Field.Keyboard),
                                        new TicTacToeRespond(Tables[id].Player2, "Клавиатура обновлена", Tables[id].Field.Keyboard),
                                    };
                                }
                            default:
                                return new[]
                                {
                                    new TicTacToeRespond(id, "Что-то пошло не так", TicTacToeKeyboards.Empty)
                                };
                        }
                        
                    case DialogStatus.Menu:
                        switch (payload)
                        {
                            case "NewRoom":
                                Dialogs[id] = DialogStatus.WaitingForNewRoomId;
                                return new[]
                                {
                                    new TicTacToeRespond(id, "Введите номер, который будет именем комнаты. Любой набор цифр\n\nИли 0, чтобы сгенерировать номер", TicTacToeKeyboards.Empty)
                                };

                            case "ConnectToRoom":
                                Dialogs[id] = DialogStatus.WaitingForExisitingRoomId;
                                return new[]
                                {
                                    new TicTacToeRespond(id, "Введите номер комнаты.\n\nИли 0, чтобы подключиться к случайной", TicTacToeKeyboards.Empty)
                                };
                            default:
                                Dialogs[id] = DialogStatus.Menu;
                                return new[]
                                {
                                    new TicTacToeRespond(id, "Ничего не понял. Абсолютно", TicTacToeKeyboards.Main)
                                };
                        }
                    case DialogStatus.WaitingForNewRoomId:
                        uint newRoomId;
                        try
                        {
                            newRoomId = Convert.ToUInt32(message);
                            if (newRoomId != 0)
                            {
                                if (RoomIdToPlayer1.ContainsKey(newRoomId))
                                {
                                    Dialogs[id] = DialogStatus.Menu;
                                    return new[] { new TicTacToeRespond(id, "Жаль, но комната с таким номером уже есть. Попробуйте другой", TicTacToeKeyboards.Main) };
                                }
                            }
                            else
                            {
                                do
                                {
                                    newRoomId++;
                                } while (RoomIdToPlayer1.ContainsKey(newRoomId));
                            }

                            RoomIdToPlayer1.Add(newRoomId, id);

                            Dialogs[id] = DialogStatus.Menu;
                            return new[] { new TicTacToeRespond(id, $"Комната с номером {newRoomId} создана. Осталось подождать, пока кто-нибудь подключится", TicTacToeKeyboards.Main) };
                        }
                        catch (Exception ex) when (ex is OverflowException || ex is FormatException)
                        {
                            Dialogs[id] = DialogStatus.Menu;
                            return new[] { new TicTacToeRespond(id, $"Какие-то странные цифры. Это что-то на японском?", TicTacToeKeyboards.Main)};
                        }
                        catch
                        {
                            Dialogs[id] = DialogStatus.Menu;
                            return new[] { new TicTacToeRespond(id, $"Что-то пошло не так. Совсем не так...", TicTacToeKeyboards.Main) };
                        }

                    case DialogStatus.WaitingForExisitingRoomId:
                        uint roomId = 0;
                        try
                        {
                            roomId = Convert.ToUInt32(message);
                            
                            if (roomId != 0)
                            {
                                if (!RoomIdToPlayer1.ContainsKey(roomId))
                                {
                                    Dialogs[id] = DialogStatus.Menu;
                                    return new[]
                                    {
                                        new TicTacToeRespond(id, "Такой комнаты нет, можете создать свою или подключиться к случайной", TicTacToeKeyboards.Main)
                                    };
                                }
                            }
                            else
                            {
                                if (RoomIdToPlayer1.Keys.Count > 0)
                                {
                                    var roomIds = RoomIdToPlayer1.Keys.ToArray();

                                    foreach (var possibleRoomId in roomIds)
                                    {
                                        if (RoomIdToPlayer1[possibleRoomId] != id)
                                        {
                                            roomId = possibleRoomId;
                                            break;
                                        }
                                    }
                                    if (roomId == 0)
                                    {
                                        return new[]
                                        {
                                            new TicTacToeRespond(id, "Никого нет в очереди. Создайте комнату сами и позовите друга", TicTacToeKeyboards.Main)
                                        };
                                    }
                                }
                                else
                                {
                                    // комнат нет
                                    Dialogs[id] = DialogStatus.Menu;
                                    return new[]
                                    {
                                        new TicTacToeRespond(id, "Никого нет в очереди. Создайте комнату сами и позовите друга", TicTacToeKeyboards.Main)
                                    };
                                }
                            }
                            
                            var newGame = new Game(RoomIdToPlayer1[roomId], id);
                            Tables.Add(newGame.Player1, newGame);
                            Tables.Add(newGame.Player2, newGame);

                            Dialogs[newGame.Player1] = DialogStatus.InGame;
                            Dialogs[newGame.Player2] = DialogStatus.InGame;

                            RoomIdToPlayer1.Remove(roomId);

                            return new[]
                            {
                                new TicTacToeRespond(newGame.Player1, $"Матч с {newGame.Player2} начался! Вы играете Х, ваш ход", newGame.Field.Keyboard),
                                new TicTacToeRespond(newGame.Player2, $"Матч с {newGame.Player1} начался! Вы играете O, ходит оппонент", newGame.Field.Keyboard)
                            };
                        }
                        catch (Exception ex) when (ex is OverflowException || ex is FormatException)
                        {
                            Dialogs[id] = DialogStatus.Menu;
                            return new[] { new TicTacToeRespond(id, $"Какие-то странные цифры. Это что-то на японском?", TicTacToeKeyboards.Main) };
                        }
                        catch
                        {
                            Dialogs[id] = DialogStatus.Menu;
                            return new[] { new TicTacToeRespond(id, $"Что-то пошло не так. Совсем не так...", TicTacToeKeyboards.Main) };
                        }
                }
            }
            else
            {
                Dialogs.Add(id, DialogStatus.Menu);
                
                return new[]
                {
                    new TicTacToeRespond(id, "Создайте новую комнату или подключитесь к существующей", TicTacToeKeyboards.Main)
                };
            }

            return new[]
            {
                new TicTacToeRespond(id, "Произошла неожиданная ошибка. Обратитесь к администратору", TicTacToeKeyboards.Empty)
            };
        }
    }
}
