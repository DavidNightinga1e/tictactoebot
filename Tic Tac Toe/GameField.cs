using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace Tic_Tac_Toe
{
    enum TicTacToeSign
    {
        X,
        O
    }

    class GameField
    {
        // Поле 3х3, где 0 - ничего, 1 - крест, 2 - нолик
        public int[,] Field { private set; get; }
        public MessageKeyboard Keyboard { private set; get; }
        public MessageKeyboardButton[,] Buttons { private set; get; }

        public GameField()
        {
            Field = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            CreateFieldKeyboard();
        }

        public void Set(int y, int x, TicTacToeSign sign)
        {
            switch (sign)
            {
                case TicTacToeSign.X:
                    Field[y, x] = 1;
                    Buttons[y, x].Action.Label = "[X]";
                    Buttons[y, x].Action.Payload = "{\"button\":\"S" + y + "_" + x + "\"}";
                    Buttons[y, x].Color = KeyboardButtonColor.Primary;
                    break;

                case TicTacToeSign.O:
                    Field[y, x] = 2;
                    Buttons[y, x].Action.Label = "[O]";
                    Buttons[y, x].Action.Payload = "{\"button\":\"S" + y + "_" + x + "\"}";
                    Buttons[y, x].Color = KeyboardButtonColor.Negative;
                    break;
            }
        }

        public int WinCheck()
        {
            // Проверка горизонталей
            for (int i = 0; i < 3; ++i)
            {
                if ((Field[i, 0] == Field[i, 1]) && (Field[i, 1] == Field[i, 2]))
                {
                    if (Field[i, 0] == 1)
                    {
                        HightlightWinningCombination(i, 0, i, 1, i, 2);
                        return 1;
                    }
                    else if (Field[i, 0] == 2)
                    {
                        HightlightWinningCombination(i, 0, i, 1, i, 2);
                        return 2;
                    }
                }
            }

            // Проверка вертикалей
            for (int i = 0; i < 3; ++i)
            {
                if ((Field[0, i] == Field[1, i]) && (Field[1, i] == Field[2, i]))
                {
                    if (Field[0, i] == 1)
                    {
                        HightlightWinningCombination(0, i, 1, i, 2, i);
                        return 1;
                    }
                    else if (Field[0, i] == 2)
                    {
                        HightlightWinningCombination(0, i, 1, i, 2, i);
                        return 2;
                    }
                }
            }

            // Проверка диагоналей
            if ((Field[0, 0] == Field[1, 1]) && (Field[1, 1] == Field[2, 2]))
            {
                if (Field[0, 0] == 1)
                {
                    HightlightWinningCombination(0, 0, 1, 1, 2, 2);
                    return 1;
                }
                else if (Field[0, 0] == 2)
                {
                    HightlightWinningCombination(0, 0, 1, 1, 2, 2);
                    return 2;
                }
            }

            if ((Field[0, 2] == Field[1, 1]) && (Field[1, 1] == Field[2, 0]))
            {
                if (Field[0, 2] == 1)
                {
                    HightlightWinningCombination(0, 2, 1, 1, 2, 0);
                    return 1;
                }
                else if (Field[0, 2] == 2)
                {
                    HightlightWinningCombination(0, 2, 1, 1, 2, 0);
                    return 2;
                }
            }

            // Проверка на ничью
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (Field[i, j] == 0)
                        return 0;
                }
            }

            return -1;
        }

        private void HightlightWinningCombination(params int[] fieldPositions)
        {
            Buttons[fieldPositions[0], fieldPositions[1]].Color = KeyboardButtonColor.Positive;
            Buttons[fieldPositions[2], fieldPositions[3]].Color = KeyboardButtonColor.Positive;
            Buttons[fieldPositions[4], fieldPositions[5]].Color = KeyboardButtonColor.Positive;
        }

        /// <summary>
        /// Создает пустую клавиатуру для поля
        /// </summary>
        public void CreateFieldKeyboard()
        {
            Keyboard = new MessageKeyboard
            {
                OneTime = false
            };
            Buttons = new MessageKeyboardButton[3, 3];
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    MessageKeyboardButton button = new MessageKeyboardButton
                    {
                        Action = new MessageKeyboardButtonAction()
                    };

                    button.Action.Label = "[   ]";
                    button.Action.Payload = "{\"button\":\"C" + i + "_" + j + "\"}";
                    button.Color = KeyboardButtonColor.Default;
                    Buttons[i, j] = button;
                }
            }

            Keyboard.Buttons = new[] {
                new[]{ Buttons[0, 0], Buttons[0, 1], Buttons[0, 2]},
                new[]{ Buttons[1, 0], Buttons[1, 1], Buttons[1, 2] },
                new[] { Buttons[2, 0], Buttons[2, 1], Buttons[2, 2] } };
        }
    }
}
