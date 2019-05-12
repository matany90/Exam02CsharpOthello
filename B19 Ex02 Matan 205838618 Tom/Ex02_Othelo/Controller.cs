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

        public static void DrawBoard(int i_BoardSize, int[,] i_Pieces, int i_PlayerTurn)
        {
            UserUI.DrawBoard(i_BoardSize, i_Pieces, i_PlayerTurn);
        }

        public static void GetTurn(ref int? o_RowIndex, ref int? o_ColIndex, List<string> i_PossibleMoves, bool i_IsTwoPlayer, int i_PlayerTurn, ref bool io_GameOver)
        {
            string inputFromUserStr = "";

            if (i_IsTwoPlayer || (!i_IsTwoPlayer && i_PlayerTurn == 0))
            {
                inputFromUserStr = UserUI.GetInputFromUser();
                inputFromUserStr = inputFromUserStr.ToUpper();
                if (inputFromUserStr != "Q")
                {
                    while (!i_PossibleMoves.Contains(inputFromUserStr))
                    {
                        UserUI.ShowMessage("Illegal move, try again");
                        inputFromUserStr = UserUI.GetInputFromUser();
                        inputFromUserStr = inputFromUserStr.ToUpper();
                    }
                }
                else
                {
                    io_GameOver = true;
                }
            }
            else
            {
                System.Threading.Thread.Sleep(1000);
                Random rnd = new Random();
                int randomIndex = rnd.Next(0, i_PossibleMoves.Count);
                inputFromUserStr = i_PossibleMoves[randomIndex];
            }

            if (!io_GameOver)
            {
                o_ColIndex = (int)(inputFromUserStr[0] - 'A' + 1) - 1;
                o_RowIndex = int.Parse(inputFromUserStr[1].ToString()) - 1;
            }
        }

        public static bool EndGame(string i_FName, string i_SName, int i_FScore, int i_SScore)
        {
            string playerChoice;

            if (i_FScore > i_SScore)
            {
                UserUI.ShowMessage("The Winner is " + i_FName);
            }
            else if (i_FScore < i_SScore)
            {
                UserUI.ShowMessage("The Winner is " + i_SName);
            }
            else
            {
                UserUI.ShowMessage("The game ended in draw");
            }

            UserUI.ShowMessage("Play again?[y/n]");
            playerChoice = UserUI.GetInputFromUser().ToLower();
            while (playerChoice != "y" && playerChoice != "n")
            {
                ShowMessage("Your input is invalid. Please try again, then press Enter");
                playerChoice = GetInputFromUser().ToLower();
            }

            return playerChoice.Equals("n");
        }
    }
}
