using System;
using System.Collections.Generic;

namespace Ex02_Othelo
{
    class Game
    {
        private string m_FirstUser;
        private string m_SecondUser;
        private int m_FirstUserScore;
        private int m_SecondUserScore;
        private int m_BoardSize;
        private bool m_IsTwoPlayer;
        private int[,] m_Board;
        private int m_PlayerTurn;
        private bool m_GameOver;

        public Game()
        {
            Controller.ShowMessage("Welcome to Othelo!");
            initUserPreferences();
            initBoard();
            //Controller.DrawBorad(m_BoardSize, m_Board, 1);
        }

        private void initUserPreferences()
        {
            Controller.ShowMessage("Please enter name for Player 1 and then press Enter: ");
            m_FirstUser = Controller.GetInputFromUser();

            Controller.ShowMessage(
@"Would you like to play against another player, or against the computer?
Please press P for a game against another player, and C for playing against the computer.
After your choose, press Enter:");
            m_IsTwoPlayer = Controller.GetTwoPlayerUserChoice();

            if (m_IsTwoPlayer)
            {
                Controller.ShowMessage("Please enter name for Player 2 and then press Enter: ");
                m_SecondUser = Controller.GetInputFromUser();
            }
            else
            {
                m_SecondUser = "Computer";
            }

            Controller.ShowMessage(
@"Please select the board size for the game.
For board size 6X6, please press button 6
For board size 8X8, please press button 8
After your choose, press Enter:");
            m_BoardSize = Controller.GetBoardSizeUserChoice();
        }

        private void initBoard()
        {
            int middleIndex = m_BoardSize / 2;
       
            m_Board = new int[m_BoardSize, m_BoardSize];
            m_Board[middleIndex, middleIndex] = 1;
            m_Board[middleIndex - 1, middleIndex - 1] = 1;
            m_Board[middleIndex, middleIndex - 1] = 2;
            m_Board[middleIndex - 1, middleIndex] = 2;
        }

        public void PlayTurn()
        {
            int rowIndex, colIndex;

            Controller.GetTurn(out rowIndex, out colIndex, CalculateMoves());
            m_Board[rowIndex, colIndex] = m_PlayerTurn + 1;
            m_PlayerTurn = (m_PlayerTurn + 1) % 2;
            Controller.DrawBoard(m_BoardSize, m_Board, m_PlayerTurn);
            if (m_PlayerTurn == 0)
            {
                m_FirstUserScore++;
            }
            else
            {
                m_SecondUserScore++;
            }
        }

        public List<string> CalculateMoves()
        {
            List<string> possibleMoves = new List<string>();

            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    if (m_Board[i, j] == 0)
                    {
                        possibleMoves.Add((char)('A' + j) + (i + 1).ToString());
                    }
                }
            }

            return possibleMoves;
        }
    }
}
