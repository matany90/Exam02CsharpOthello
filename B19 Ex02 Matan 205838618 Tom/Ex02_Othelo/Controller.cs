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

        public static bool GetTwoPlayerUserChoice()
        {
            string playerChoice = GetInputFromUser().ToLower();

            while (playerChoice != "p" && playerChoice != "c")
            {
                ShowMessage("Your input is invalid. Please try again, then press Enter");
                playerChoice = GetInputFromUser().ToLower();
            }

            return playerChoice.Equals("p");
        }

        public static int GetBoardSizeUserChoice()
        {
            string playerChoice = GetInputFromUser();

            while (playerChoice != "6" && playerChoice != "8")
            {
                ShowMessage("Your input is invalid. Please try again, then press Enter");
                playerChoice = GetInputFromUser();
            }

            return int.Parse(playerChoice);
        }

        public static void DrawBorad(int i_BoardSize, int[,] i_Pieces, int i_PlayerTurn)
        {
            UserUI.DrawBoard(i_BoardSize, i_Pieces, i_PlayerTurn);
        }
    }
}
