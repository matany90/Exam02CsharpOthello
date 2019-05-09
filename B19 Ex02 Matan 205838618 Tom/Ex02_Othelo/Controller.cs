using System;

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

        public static void DrawBorad(int i_BoardSize, int[,] i_Pieces, int i_PlayerTurn)
        {
            UserUI.DrawBoard(i_BoardSize, i_Pieces, i_PlayerTurn);
        }
    }
}
