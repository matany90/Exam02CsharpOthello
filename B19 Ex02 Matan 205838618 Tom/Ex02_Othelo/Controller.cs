using System;
using System.Collections.Generic;

namespace Ex02_Othelo
{
    internal class Controller
    {
        private Game m_Game = new Game();

        public int GetFirstUserScore()
        {
            return m_Game.FirstUserScore;
        }

        public int GetSecondUserScore()
        {
            return m_Game.SecondUserScore;
        }

        public bool SetIsTwoPlayer(string i_Choise)
        {
            bool isValidInput = true;

            if (i_Choise != "p" && i_Choise != "c")
            {
                isValidInput = false;
            }
            else
            {
                if (i_Choise.Equals("p"))
                {
                    m_Game.IsTwoPlayer = true;
                }
                else
                {
                    m_Game.IsTwoPlayer = false;
                }
            }

            return isValidInput;
        }

        public bool GetIsTwoPlayer()
        {
            return m_Game.IsTwoPlayer;
        }

        public bool SetBoardSize(string i_Choise)
        {
            bool isValidInput = true;

            if (i_Choise != "6" && i_Choise != "8")
            {
                isValidInput = false;
            }
            else
            {
                m_Game.BoardSize = int.Parse(i_Choise);
            }

            return isValidInput;
        }

        public int GetBoardSize()
        {
            return m_Game.BoardSize;
        }

        public int[,] GetBoard()
        {
            return m_Game.Board;
        }

        public int GetPlayerTurn()
        {
            return m_Game.PlayerTurn;
        }

        public bool IsGameOver()
        {
            return m_Game.GameOver;
        }

        public void SetGameOver(bool i_Val)
        {
            m_Game.GameOver = i_Val;
        }

        public bool GetAvailableMoveFirstPlayer()
        {
            return m_Game.AvailableMoveFirstPlayer;
        }

        public void SetAvailableMoveFirstPlayer(bool i_Val)
        {
            m_Game.AvailableMoveFirstPlayer = i_Val;
        }

        public bool GetAvailableMoveSecondPlayer()
        {
            return m_Game.AvailableMoveSecondPlayer;
        }

        public void SetAvailableMoveSecondPlayer(bool i_Val)
        {
            m_Game.AvailableMoveSecondPlayer = i_Val;
        }

        public void InitBoard()
        {
            m_Game.InitBoard();
        }

        public bool CheckAvailableMoves()
        {
            bool isAvailableMoves = true;
            List<string> possibleMoves = m_Game.CalculateMoves();

            if (possibleMoves.Count == 0)
            {
                isAvailableMoves = false;
            }

            return isAvailableMoves;
        }

        public bool GetTurn(string i_Move = "")
        {
            bool isValidInput = true;
            int rowIndex, colIndex;
            List<string> possibleMoves = m_Game.CalculateMoves();

            if (!m_Game.IsTwoPlayer && m_Game.PlayerTurn == 1)
            {
                System.Threading.Thread.Sleep(1000);
                Random rnd = new Random();
                int randomIndex = rnd.Next(0, possibleMoves.Count);
                i_Move = possibleMoves[randomIndex];
            }

            if (!possibleMoves.Contains(i_Move))
            {
                isValidInput = false;
            }
            else
            {
                colIndex = (int)(i_Move[0] - 'A' + 1) - 1;
                rowIndex = int.Parse(i_Move[1].ToString()) - 1;
                m_Game.PlayTurn(rowIndex, colIndex);
            }

            return isValidInput;
        }

        public void NextTurn()
        {
            m_Game.NextTurn();
        }
        
        public bool IsPlayAgain(string i_Choise)
        {
            return i_Choise.Equals("y");
        }
    }
}
