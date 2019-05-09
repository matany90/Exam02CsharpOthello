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
            Controller.ShowMessage(string.Format(
@"Welcome to Othelo!
Please Write your Name and then press Enter:"
));
            m_FirstUser = Controller.GetInputFromUser();

 
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
