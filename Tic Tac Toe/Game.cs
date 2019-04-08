using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Keyboard;
using VkNet.Enums.SafetyEnums;

namespace Tic_Tac_Toe
{
    enum ButtonPushResult
    {
        Set,
        Taken,
        NotYourTurn
    };

    class Game
    {
        public long Player1 { private set; get; }
        public long Player2 { private set; get; }
        long CurrentPlayer;
        public GameField Field { private set; get; }
        public bool IsEnded { private set; get; }
        public long Winner { private set; get; }
        public long Lost { private set; get; }

        public Game(long player1, long player2)
        {
            Player1 = player1;
            Player2 = player2;
            CurrentPlayer = player1;
            IsEnded = false;
            Winner = 0;
            Lost = 0;
            Field = new GameField();
        }

        /// <summary>
        /// Применяет изменения на поле по нажатию кнопки и создает ответ для пользователя
        /// </summary>
        /// <param name="payload">Нажатая кнопка</param>
        /// <returns>Ответ</returns>
        public ButtonPushResult ButtonPush(long id, string payload)
        {
            int i = Convert.ToInt32(payload[12]) - 48;
            int j = Convert.ToInt32(payload[14]) - 48;

            if (id == CurrentPlayer)
            {
                if (payload[11] == 'C')
                {
                    if (CurrentPlayer == Player1)
                        Field.Set(i, j, TicTacToeSign.X);
                    else
                        Field.Set(i, j, TicTacToeSign.O);

                    CheckForEnding();

                    // Если ход совершил игрок 1, то передаем ход игроку 2. И наоборот
                    CurrentPlayer = Player1 == id ? Player2 : Player1;
                    return ButtonPushResult.Set;
                }
                else
                {
                    return ButtonPushResult.Taken;
                }
            }
            else
            {
                return ButtonPushResult.NotYourTurn;
            }
        }

        private void CheckForEnding()
        {
            switch (Field.WinCheck())
            {
                case -1:
                    IsEnded = true;
                    break;
                case 1:
                    IsEnded = true;
                    Winner = Player1;
                    Lost = Player2;
                    break;
                case 2:
                    IsEnded = true;
                    Winner = Player2;
                    Lost = Player1;
                    break;
                default:
                    break;
            }
        }

        public void RestartAndSwap()
        {
            Field = new GameField();

            IsEnded = false;
            Winner = 0;
            Lost = 0;

            var t = Player1;
            Player1 = Player2;
            Player2 = t;
        }
    }
}
