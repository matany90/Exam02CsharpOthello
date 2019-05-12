using System;

namespace Ex02_Othelo
{
    internal class UserUI
    {
        public static void ShowMessage(string i_MessageToShow)
        {
            Console.WriteLine(i_MessageToShow);
        }

        public static string GetInputFromUser()
        {
            return Console.ReadLine();
        }

        public static void DrawBoard(int i_BoardSize, int[,] i_Pieces, int i_PlayerTurn)
        {
            char colMark = 'A';
            int rowMark = 1;
            string board = "    ";

            Ex02.ConsoleUtils.Screen.Clear();
            for (int i = 0; i < i_BoardSize; i++)
            {
                board += colMark + "   ";
                colMark++;
            }

            board += Environment.NewLine + "   " + new string('=', 4 * i_BoardSize) + Environment.NewLine;
            for (int i = 0; i < i_BoardSize; i++)
            {
                board += rowMark.ToString() + " ";
                for (int j = 0; j < i_BoardSize; j++)
                {
                    switch (i_Pieces[i, j])
                    {
                        case 1:
                            board += "| O ";
                            break;
                        case 2:
                            board += "| X ";
                            break;
                        default:
                            board += "|   ";
                            break;
                    }
                }

                board += "|" + Environment.NewLine + "   " + new string('=', 4 * i_BoardSize) + Environment.NewLine;
                rowMark++;
            }

            board += "Player " + (i_PlayerTurn + 1) + " Turn" + Environment.NewLine;
            Console.Write(board);
        }
    }
}
