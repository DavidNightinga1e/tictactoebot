using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{

    class TicTacToe
    {
        enum DialogStatus
        {
            Menu,
            WaitingForNewRoomId,
            WaitingForExisitingRoomId,
            InGame
        };

        Dictionary<long, DialogStatus> Dialogs;
        Dictionary<uint, long> RoomIdToPlayer1;
        Dictionary<long, Game> Rooms;

        TicTacToeKeyboards TicTacToeKeyboards;

        TicTacToe()
        {
            Dialogs = new Dictionary<long, DialogStatus>();
            RoomIdToPlayer1 = new Dictionary<uint, long>();
            TicTacToeKeyboards = new TicTacToeKeyboards();
            Rooms = new Dictionary<long, Game>();
        }

        public TicTacToeRespond Respond(long id, string message, string payload)
        {
            payload = payload.Substring(10, payload.Length - 13);
            
            if (Dialogs.ContainsKey(id))
            {
                switch (Dialogs[id])
                {
                    case DialogStatus.InGame:
                        // todo обработка payload для изменения игрового поля
                        break;
                    case DialogStatus.Menu:
                        // todo изменение статуса на ожидание ввода в зависимости от кнопки
                        break;
                    case DialogStatus.WaitingForNewRoomId:
                        uint newRoomId;
                        try
                        {
                            newRoomId = Convert.ToUInt32(message);
                            if (newRoomId != 0)
                            {
                                if (RoomIdToPlayer1.ContainsKey(newRoomId))
                                {
                                    return new TicTacToeRespond("Жаль, но комната с таким номером уже есть. Попробуйте другой", TicTacToeKeyboards.Main);
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
                            return new TicTacToeRespond($"Комната с номером {newRoomId} создана. Осталось подождать, пока кто-нибудь подключится", TicTacToeKeyboards.Main);
                        }
                        catch (Exception ex) when (ex is OverflowException || ex is FormatException)
                        {
                            return new TicTacToeRespond($"Какие-то странные цифры. Это что-то на японском?", TicTacToeKeyboards.Main);
                        }
                        catch
                        {
                            return new TicTacToeRespond($"Что-то пошло не так. Совсем не так...", TicTacToeKeyboards.Main);
                        }

                    case DialogStatus.WaitingForExisitingRoomId:
                        // todo подключение к ближайшей комнате, подключение к нужной комнате
                        break;
                }
            }
        }
    }
}
