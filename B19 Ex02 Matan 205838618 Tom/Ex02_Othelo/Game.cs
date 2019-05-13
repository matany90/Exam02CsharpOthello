using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ex02_Othelo
{
    internal class Game
    {
        private int m_FirstUserScore;
        private int m_SecondUserScore;
        private int m_BoardSize;
        private bool m_IsTwoPlayer;
        private int[,] m_Board;
        private int m_PlayerTurn;
        private bool m_GameOver;
        private bool m_IsAvaliableMoveFirstPlayer = true;
        private bool m_IsAvaliableMoveSecondPlayer = true;

        public int FirstUserScore
        {
            get { return m_FirstUserScore; }
            set { m_FirstUserScore = value; }
        }

        public int SecondUserScore
        {
            get { return m_SecondUserScore; }
            set { m_SecondUserScore = value; }
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
            set { m_BoardSize = value; }
        }

        public bool IsTwoPlayer
        {
            get { return m_IsTwoPlayer; }
            set { m_IsTwoPlayer = value; }
        }

        public int[,] Board
        {
            get { return m_Board; }
        }

        public int PlayerTurn
        {
            get { return m_PlayerTurn; }
            set { m_PlayerTurn = value; }
        }

        public bool GameOver
        {
            get { return m_GameOver; }
            set { m_GameOver = value; }
        }

        public bool AvailableMoveFirstPlayer
        {
            get { return m_IsAvaliableMoveFirstPlayer; }
            set { m_IsAvaliableMoveFirstPlayer = value; }
        }

        public bool AvailableMoveSecondPlayer
        {
            get { return m_IsAvaliableMoveSecondPlayer; }
            set { m_IsAvaliableMoveSecondPlayer = value; }
        }
        
        public void InitBoard()
        {
            int middleIndex = m_BoardSize / 2;

            m_Board = new int[m_BoardSize, m_BoardSize];
            m_Board[middleIndex, middleIndex] = 1;
            m_Board[middleIndex - 1, middleIndex - 1] = 1;
            m_Board[middleIndex, middleIndex - 1] = 2;
            m_Board[middleIndex - 1, middleIndex] = 2;
            m_FirstUserScore = 2;
            m_SecondUserScore = 2;
            m_PlayerTurn = 0;
            m_GameOver = false;
            m_IsAvaliableMoveFirstPlayer = true;
            m_IsAvaliableMoveSecondPlayer = true;
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
                    IsTwoPlayer = true;
                }
                else
                {
                    IsTwoPlayer = false;
                }
            }

            return isValidInput;
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
                BoardSize = int.Parse(i_Choise);
            }

            return isValidInput;
        }

        public bool IsPlayAgain(string i_Choise)
        {
            return i_Choise.Equals("y");
        }

        public bool CheckAvailableMoves()
        {
            bool isAvailableMoves = true;
            List<string> possibleMoves = CalculateMoves();

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
            List<string> possibleMoves = CalculateMoves();

            if (IsComputerTurn())
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
                PlayTurn(rowIndex, colIndex);
            }

            return isValidInput;
        }

        public void CheckIfUserWantToExit(string i_Choise)
        {
             if (i_Choise.Equals("Q"))
            {
                m_GameOver = true;
            }
        }

        public void CheckIfNoAvailableMovesForBothPlayers()
        {
            if (!m_IsAvaliableMoveFirstPlayer && !m_IsAvaliableMoveSecondPlayer)
            {
                GameOver = true;
            }
        }

        public bool IsNoAvailableMovesForOnePlayer()
        {
            return (!m_IsAvaliableMoveFirstPlayer || !m_IsAvaliableMoveSecondPlayer) && GameOver == false;
        }

        public bool IsFirstPlayerWon()
        {
            return m_FirstUserScore > m_SecondUserScore;
        }

        public bool IsSecondPlayerWon()
        {
            return m_FirstUserScore < m_SecondUserScore;
        }

        public bool IsUserWantToExit(string i_Choise)
        {
            return i_Choise.Equals("Q");
        }

        public void HandleNoAvailableMoves()
        {
            if (FirstUserScore + SecondUserScore == BoardSize * BoardSize)
            {
                GameOver = true;
            }
            else
            {
                if (PlayerTurn == 0)
                {
                    AvailableMoveFirstPlayer = false;
                }
                else
                {
                    AvailableMoveSecondPlayer = false;
                }
            }
        }

        public bool IsComputerTurn()
        {
            return !IsTwoPlayer && PlayerTurn == 1; 
        }

        public void PlayTurn(int i_Row, int i_Col)
        {
            m_Board[i_Row, i_Col] = m_PlayerTurn + 1;
            UpdateBoard(i_Row, i_Col);
            m_IsAvaliableMoveSecondPlayer = true;
            m_IsAvaliableMoveFirstPlayer = true;
            if (m_PlayerTurn == 0)
            {
                m_FirstUserScore++;
            }
            else
            {
                m_SecondUserScore++;
            }

            NextTurn();
        }

        public void NextTurn()
        {
            m_PlayerTurn = (m_PlayerTurn + 1) % 2;
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
                        if (IsPossibleMove(i, j))
                        {
                            possibleMoves.Add((char)('A' + j) + (i + 1).ToString());
                        }
                    }
                }
            }

            return possibleMoves;
        }

        private bool IsPossibleMove(int i_Row, int i_Col)
        {
            bool legalMove = false;
            List<string> regexExpression = new List<string>();

            m_Board[i_Row, i_Col] = m_PlayerTurn + 1;
            regexExpression.Add(checkRight(i_Row, i_Col));
            regexExpression.Add(checkLeft(i_Row, i_Col));
            regexExpression.Add(checkDown(i_Row, i_Col));
            regexExpression.Add(checkUp(i_Row, i_Col));
            regexExpression.Add(checkBottomRight(i_Row, i_Col));
            regexExpression.Add(checkBottomLeft(i_Row, i_Col));
            regexExpression.Add(checkTopRight(i_Row, i_Col));
            regexExpression.Add(checkTopLeft(i_Row, i_Col));

            foreach (string expression in regexExpression)
            {
                if (m_PlayerTurn == 0)
                {
                    legalMove = Regex.IsMatch(expression, @"\b0*12+1+2*1*0*\b");
                    if (legalMove)
                    {
                        ////test print
                        Console.WriteLine("Expression " + expression + " for index: " + (char)('A' + i_Col) + (i_Row + 1).ToString());
                        ////test print
                        break;
                    }
                }
                else
                {
                    legalMove = Regex.IsMatch(expression, @"\b0*21+2+1*2*0*\b");
                    if (legalMove)
                    {
                        ////test print
                        Console.WriteLine("Expression " + expression + " for index: " + (char)('A' + i_Col) + (i_Row + 1).ToString());
                        ////test print
                        break;
                    }
                }
            }

            m_Board[i_Row, i_Col] = 0;

            return legalMove;
        }

        private string checkRight(int i_Row, int i_Col)
        {
            string rightString = string.Empty;
            while (i_Col < m_BoardSize)
            {
                rightString += m_Board[i_Row, i_Col].ToString();
                i_Col++;
            }

            return rightString;
        }

        private string checkLeft(int i_Row, int i_Col)
        {
            string leftString = string.Empty;

            while (i_Col >= 0)
            {
                leftString += m_Board[i_Row, i_Col].ToString();
                i_Col--;
            }

            return leftString;
        }

        private string checkDown(int i_Row, int i_Col)
        {
            string downString = string.Empty;

            while (i_Row >= 0)
            {
                downString += m_Board[i_Row, i_Col].ToString();
                i_Row--;
            }

            return downString;
        }

        private string checkUp(int i_Row, int i_Col)
        {
            string upString = string.Empty;

            while (i_Row < m_BoardSize)
            {
                upString += m_Board[i_Row, i_Col].ToString();
                i_Row++;
            }

            return upString;
        }

        private string checkBottomRight(int i_Row, int i_Col)
        {
            string bottomRightString = string.Empty;

            while (i_Row < m_BoardSize && i_Col < m_BoardSize)
            {
                bottomRightString += m_Board[i_Row, i_Col].ToString();
                i_Row++;
                i_Col++;
            }

            return bottomRightString;
        }

        private string checkBottomLeft(int i_Row, int i_Col)
        {
            string bottomLeftString = string.Empty;

            while (i_Row < m_BoardSize && i_Col >= 0)
            {
                bottomLeftString += m_Board[i_Row, i_Col].ToString();
                i_Row++;
                i_Col--;
            }

            return bottomLeftString;
        }

        private string checkTopRight(int i_Row, int i_Col)
        {
            string topRightString = string.Empty;

            while (i_Row >= 0 && i_Col < m_BoardSize)
            {
                topRightString += m_Board[i_Row, i_Col].ToString();
                i_Row--;
                i_Col++;
            }

            return topRightString;
        }

        private string checkTopLeft(int i_Row, int i_Col)
        {
            string topLeftString = string.Empty;

            while (i_Row >= 0 && i_Col >= 0)
            {
                topLeftString += m_Board[i_Row, i_Col].ToString();
                i_Row--;
                i_Col--;
            }

            return topLeftString;
        }

        private void UpdateBoard(int i_Row, int i_Col)
        {
            bool legalMove;
            string[] directions = { "Right", "Left", "Down", "Up", "DownRight", "DownLeft", "UpRight", "UpLeft" };
            int directionIndex = 0;
            List<string> regexExpression = new List<string>();

            regexExpression.Add(checkRight(i_Row, i_Col));
            regexExpression.Add(checkLeft(i_Row, i_Col));
            regexExpression.Add(checkDown(i_Row, i_Col));
            regexExpression.Add(checkUp(i_Row, i_Col));
            regexExpression.Add(checkBottomRight(i_Row, i_Col));
            regexExpression.Add(checkBottomLeft(i_Row, i_Col));
            regexExpression.Add(checkTopRight(i_Row, i_Col));
            regexExpression.Add(checkTopLeft(i_Row, i_Col));

            foreach (string expression in regexExpression)
            {
                if (m_PlayerTurn == 0)
                {
                    legalMove = Regex.IsMatch(expression, @"\b0*12+1+2*1*0*\b");
                    if (legalMove)
                    {
                        UpdateLine(i_Row, i_Col, directions[directionIndex], ref m_FirstUserScore, ref m_SecondUserScore);
                    }
                }
                else
                {
                    legalMove = Regex.IsMatch(expression, @"\b0*21+2+1*2*0*\b");
                    if (legalMove)
                    {
                        UpdateLine(i_Row, i_Col, directions[directionIndex], ref m_SecondUserScore, ref m_FirstUserScore);
                    }
                }

                directionIndex++;
            }
        }

        private void UpdateLine(int i_Row, int i_Col, string i_Direction, ref int io_AddPlayerScore, ref int io_DecPlayerScore)
        {
            switch (i_Direction)
            {
                case "Right":
                    while (m_Board[i_Row, i_Col + 1] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row, i_Col + 1] = m_PlayerTurn + 1;
                        i_Col++;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                case "Left":
                    while (m_Board[i_Row, i_Col - 1] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row, i_Col - 1] = m_PlayerTurn + 1;
                        i_Col--;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                case "Up":
                    while (m_Board[i_Row + 1, i_Col] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row + 1, i_Col] = m_PlayerTurn + 1;
                        i_Row++;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                case "Down":
                    while (m_Board[i_Row - 1, i_Col] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row - 1, i_Col] = m_PlayerTurn + 1;
                        i_Row--;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                case "UpRight":
                    while (m_Board[i_Row - 1, i_Col + 1] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row - 1, i_Col + 1] = m_PlayerTurn + 1;
                        i_Row--;
                        i_Col++;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                case "UpLeft":
                    while (m_Board[i_Row - 1, i_Col - 1] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row - 1, i_Col - 1] = m_PlayerTurn + 1;
                        i_Row--;
                        i_Col--;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                case "DownRight":
                    while (m_Board[i_Row + 1, i_Col + 1] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row + 1, i_Col + 1] = m_PlayerTurn + 1;
                        i_Row++;
                        i_Col++;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                case "DownLeft":
                    while (m_Board[i_Row + 1, i_Col - 1] != m_PlayerTurn + 1)
                    {
                        m_Board[i_Row + 1, i_Col - 1] = m_PlayerTurn + 1;
                        i_Row++;
                        i_Col--;
                        io_AddPlayerScore++;
                        io_DecPlayerScore--;
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
