using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        private bool m_IsAvaliableMoveFirstPlayer = true;
        private bool m_IsAvaliableMoveSecondPlayer = true;

        public Game()
        {
            Controller.ShowMessage("Welcome to Othelo!");
            initUserPreferences();
            StartGame();
        }

        private void StartGame()
        {
            initBoard();
            Controller.DrawBoard(m_BoardSize, m_Board, m_PlayerTurn);
            UserUI.ShowMessage("F:" + m_FirstUserScore + " S:" + m_SecondUserScore);
            while (!m_GameOver)
            {
                PlayTurn();
            }

            if (Controller.EndGame(m_FirstUser, m_SecondUser, m_FirstUserScore, m_SecondUserScore))
            {
                Controller.ShowMessage("Thanks for playing!");
            }
            else
            {
                StartGame();
            }
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
            //m_Board[0, 5] = 2;
            // m_Board[0, 0] = 0;
            //m_Board[0, 1] = 0;
            m_FirstUserScore = 2;
            m_SecondUserScore = 2;
            m_PlayerTurn = 0;
            m_GameOver = false;
        }

        private void PlayTurn()
        {
            int rowIndex, colIndex;
            List<string> possibleMoves = CalculateMoves();
            if (possibleMoves.Count == 0)
            {
                if (m_FirstUserScore + m_SecondUserScore == m_BoardSize * m_BoardSize)
                {
                    m_GameOver = true;
                    m_IsAvaliableMoveFirstPlayer = true;
                    m_IsAvaliableMoveSecondPlayer = true;
                }
                else
                {
                    if (m_PlayerTurn == 0)
                    {
                        m_IsAvaliableMoveFirstPlayer = false;
                    } 
                    else
                    {
                        m_IsAvaliableMoveSecondPlayer = false;
                    }
                }
                m_PlayerTurn = (m_PlayerTurn + 1) % 2;              
            }
            else
            {
                Controller.GetTurn(out rowIndex, out colIndex, possibleMoves, m_IsTwoPlayer, m_PlayerTurn);
                m_Board[rowIndex, colIndex] = m_PlayerTurn + 1;
                UpdateBoard(rowIndex, colIndex);
                if (m_PlayerTurn == 0)
                {
                    m_FirstUserScore++;
                }
                else
                {
                    m_SecondUserScore++;
                }
                m_PlayerTurn = (m_PlayerTurn + 1) % 2;
            }
            Controller.DrawBoard(m_BoardSize, m_Board, m_PlayerTurn);

            if (!m_IsAvaliableMoveFirstPlayer && !m_IsAvaliableMoveSecondPlayer)
            {
                m_GameOver = true;
                m_IsAvaliableMoveFirstPlayer = true;
                m_IsAvaliableMoveSecondPlayer = true;
            }
            else if (!m_IsAvaliableMoveFirstPlayer || !m_IsAvaliableMoveSecondPlayer)
            {
                Controller.ShowMessage(string.Format(
@"No move is available for Player {0}, 
the turn goes to Player {1}", ((m_PlayerTurn + 1) % 2) + 1, m_PlayerTurn + 1));
            }
            Controller.ShowMessage("F:" + m_FirstUserScore + " S:" + m_SecondUserScore);
        }

        private List<string> CalculateMoves()
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
                        //test print
                        Console.WriteLine("Expression " + expression + " for index: " + (char)('A' + i_Col) + (i_Row + 1).ToString());
                        //test print
                        break;
                    }
                }
                else
                {
                    legalMove = Regex.IsMatch(expression, @"\b0*21+2+1*2*0*\b");
                    if (legalMove)
                    {
                        //test print
                        Console.WriteLine("Expression " + expression + " for index: " + (char)('A' + i_Col) + (i_Row + 1).ToString());
                        //test print
                        break;
                    }
                }
            }
            m_Board[i_Row, i_Col] = 0;

            return legalMove;
        }

        private string checkRight(int i_Row, int i_Col)
        {
            string rightString = "";
            while (i_Col < m_BoardSize)
            {
                rightString += m_Board[i_Row, i_Col].ToString();
                i_Col++;
            }

            return rightString;
        }

        private string checkLeft(int i_Row, int i_Col)
        {
            string leftString = "";

            while (i_Col >= 0)
            {
                leftString += m_Board[i_Row, i_Col].ToString();
                i_Col--;
            }

            return leftString;
        }
        private string checkDown(int i_Row, int i_Col)
        {
            string downString = "";

            while (i_Row >= 0)
            {
                downString += m_Board[i_Row, i_Col].ToString();
                i_Row--;
            }

            return downString;
        }

        private string checkUp(int i_Row, int i_Col)
        {
            string upString = "";

            while (i_Row < m_BoardSize)
            {
                upString += m_Board[i_Row, i_Col].ToString();
                i_Row++;
            }

            return upString;
        }

        private string checkBottomRight(int i_Row, int i_Col)
        {
            string bottomRightString = "";

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
            string bottomLeftString = "";

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
            string topRightString = "";

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
            string topLeftString = "";

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
