using System;
using System.Collections.Generic;

namespace Ex02_Othelo
{
    class Controller
    {
        public static void ShowMessage(string i_MessageToShow)
        {
            UserUI.ShowMessage(i_MessageToShow);
        }

        public static string GetInputFromUser()
        {
            return UserUI.GetInputFromUser();
        }

        public static void DrawBoard(int i_BoardSize, int[,] i_Pieces, int i_PlayerTurn)
        {
            UserUI.DrawBoard(i_BoardSize, i_Pieces, i_PlayerTurn);
        }

        public static void GetTurn(out int o_RowIndex, out int o_ColIndex, List<string> i_PossibleMoves)
        {
            string inputFromUserStr = "";

            inputFromUserStr = UserUI.GetInputFromUser();
            inputFromUserStr = inputFromUserStr.ToUpper();
            while (!i_PossibleMoves.Contains(inputFromUserStr))
            {
                UserUI.ShowMessage("Illegal move, try again");
                inputFromUserStr = UserUI.GetInputFromUser();
                inputFromUserStr = inputFromUserStr.ToUpper();
            }

            o_ColIndex = (int)(inputFromUserStr[0] - 'A' + 1) - 1;
            o_RowIndex = int.Parse(inputFromUserStr[1].ToString()) - 1;
        }
    }
}
