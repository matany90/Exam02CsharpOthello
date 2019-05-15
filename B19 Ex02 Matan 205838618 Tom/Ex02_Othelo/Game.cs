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
        private bool m_IsAvailableMoveFirstPlayer = true;
        private bool m_IsAvailableMoveSecondPlayer = true;
        List<string> m_PossibleMoves = new List<string>();

        ////Get'rs & Set'rs
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

        public List<string> PossibleMoves
        {
            get { return m_PossibleMoves; }
            set { m_PossibleMoves = value; }
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
            get { return m_IsAvailableMoveFirstPlayer; }
            set { m_IsAvailableMoveSecondPlayer = value; }
        }

        public bool AvailableMoveSecondPlayer
        {
            get { return m_IsAvailableMoveFirstPlayer; }
            set { m_IsAvailableMoveSecondPlayer = value; }
        }

        ////Initialize Board
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
            m_IsAvailableMoveFirstPlayer = true;
            m_IsAvailableMoveSecondPlayer = true;
            CalculateMoves();
        }

        ////Check input validation and set IsTwoPlayer
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
                    m_IsTwoPlayer = true;
                }
                else
                {
                    m_IsTwoPlayer = false;
                }
            }

            return isValidInput;
        }

        ////Check input validation and set BoardSize
        public bool SetBoardSize(string i_Choise)
        {
            bool isValidInput = true;

            if (i_Choise != "6" && i_Choise != "8")
            {
                isValidInput = false;
            }
            else
            {
                m_BoardSize = int.Parse(i_Choise);
            }

            return isValidInput;
        }

        ////Check if player wants to play again
        public bool IsPlayAgain(string i_Choise)
        {
            return i_Choise.Equals("y");
        }

        ////Returns true if there are moves available for the current player
        public bool CheckAvailableMoves()
        {
            bool isAvailableMoves = true;
            //CalculateMoves();

            if (m_PossibleMoves.Count == 0)
            {
                isAvailableMoves = false;
            }

            return isAvailableMoves;
        }

        ////Takes a move from Player and puts a mark in the appropriate place.
        ////If the player is the computer, a move is selected by AI (from possibleMoves)
        ////If the player is not the computer, the program will check move validation (if contains in possibleMoves)
        public bool GetTurn(string i_Move = "")
        {
            bool isValidInput = true;
            int rowIndex, colIndex;
            CalculateMoves();

            if (IsComputerTurn())
            {
                i_Move = computerBestChoiseAI();
                System.Threading.Thread.Sleep(1000);
                
            }

            if (!m_PossibleMoves.Contains(i_Move))
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

        ////Calculates the best move for the computer from possible moves
        ////Selects the move that will reduce maximum points from the first player
        private string computerBestChoiseAI()
        {
            int[,] originalBoard = (int[,])m_Board.Clone();
            int originalFirstUserScore = m_FirstUserScore;
            int originalSecondUserScore = m_SecondUserScore;
            int maxDifference = 0, scoreBeforeUpdateBoard, scoreAfterUpdateBoard, scoreDifference;
            List<string> bestMoves = new List<string>();

            foreach (string move in m_PossibleMoves)
            {
                int colIndex = (int)(move[0] - 'A' + 1) - 1;
                int rowIndex = int.Parse(move[1].ToString()) - 1;

                scoreBeforeUpdateBoard = m_FirstUserScore;
                m_Board[rowIndex, colIndex] = 2;
                updateBoard(rowIndex, colIndex);
                scoreAfterUpdateBoard = m_FirstUserScore;
                scoreDifference = scoreBeforeUpdateBoard - scoreAfterUpdateBoard;
                if (scoreDifference > maxDifference)
                {
                    bestMoves.Clear();
                    maxDifference = scoreDifference;
                    bestMoves.Add((char)('A' + colIndex) + (rowIndex + 1).ToString());
                }
                else if (maxDifference == scoreDifference)
                {
                    bestMoves.Add((char)('A' + colIndex) + (rowIndex + 1).ToString());
                }
                m_Board = (int[,])originalBoard.Clone();
                m_FirstUserScore = originalFirstUserScore;
                m_SecondUserScore = originalSecondUserScore;
            }
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, bestMoves.Count);

            return bestMoves[randomIndex];
        }

        ////Checks if the Player wants to exit 
        ////If returns true, game is over
        public void CheckIfUserWantToExit(string i_Choise)
        {
             if (i_Choise.Equals("Q"))
            {
                m_GameOver = true;
            }
        }

        ////Checks if there are no more moves for both players
        ////If returns true, game is over
        public void CheckIfNoAvailableMovesForBothPlayers()
        {
            if (!m_IsAvailableMoveFirstPlayer && !m_IsAvailableMoveSecondPlayer)
            {
                GameOver = true;
            }
        }

        ////Returns true if only one of the players has no available move
        public bool IsNoAvailableMovesForOnePlayer()
        {
            return (!m_IsAvailableMoveFirstPlayer || !m_IsAvailableMoveSecondPlayer) && GameOver == false;
        }

        ////Check if First Player won
        public bool IsFirstPlayerWon()
        {
            return m_FirstUserScore > m_SecondUserScore;
        }

        ////Check if Second Player won
        public bool IsSecondPlayerWon()
        {
            return m_FirstUserScore < m_SecondUserScore;
        }

        ////Checks if the exit button is selected
        public bool IsUserWantToExit(string i_Choise)
        {
            return i_Choise.Equals("Q");
        }

        ////Handle a situation where there are no available moves
        ////If the borad is full, game is over
        ////else, set m_IsAvailableMove for relevant player to false
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
                    m_IsAvailableMoveFirstPlayer = false;
                }
                else
                {
                    m_IsAvailableMoveSecondPlayer = false;
                }
            }
        }

        ////Returns true if it's computer turn to play
        public bool IsComputerTurn()
        {
            return !IsTwoPlayer && PlayerTurn == 1; 
        }

        public void PlayTurn(int i_Row, int i_Col)
        {
            m_Board[i_Row, i_Col] = m_PlayerTurn + 1;
            updateBoard(i_Row, i_Col);
            m_IsAvailableMoveFirstPlayer = true;
            m_IsAvailableMoveSecondPlayer = true;
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

        ////Move to next turn
        public void NextTurn()
        {
            m_PlayerTurn = (m_PlayerTurn + 1) % 2;
            CalculateMoves();
        }

        ////Returns a list of valid cells
        public void CalculateMoves()
        {
            m_PossibleMoves.Clear();

            for (int i = 0; i < m_BoardSize; i++)
            {
                for (int j = 0; j < m_BoardSize; j++)
                {
                    if (m_Board[i, j] == 0)
                    {
                        if (IsPossibleMove(i, j))
                        {
                            m_PossibleMoves.Add((char)('A' + j) + (i + 1).ToString());
                        }
                    }
                }
            }
        }

        ////Accepts row and column and returns whether this is a possible cell for the current player
        ////Uses regular expressions to check whether the cell is valid
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
                        //////test print
                        //Console.WriteLine("Expression " + expression + " for index: " + (char)('A' + i_Col) + (i_Row + 1).ToString());
                        //////test print
                        break;
                    }
                }
                else
                {
                    legalMove = Regex.IsMatch(expression, @"\b0*21+2+1*2*0*\b");
                    if (legalMove)
                    {
                        //////test print
                        //Console.WriteLine("Expression " + expression + " for index: " + (char)('A' + i_Col) + (i_Row + 1).ToString());
                        //////test print
                        break;
                    }
                }
            }

            m_Board[i_Row, i_Col] = 0;

            return legalMove;
        }

        ////Returns right-path string from specific cell
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

        ////Returns left-path string from specific cell
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

        ////Returns down-path string from specific cell
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

        ////Returns up-path string from specific cell
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

        ////Returns bottom-right path string from specific cell
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

        ////Returns bottom-left path string from specific cell
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

        ////Returns top-right path string from specific cell
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

        ////Returns top-left path string from specific cell
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

        ////Updates the board after Player has selected a move
        private void updateBoard(int i_Row, int i_Col)
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
                        updateLine(i_Row, i_Col, directions[directionIndex], ref m_FirstUserScore, ref m_SecondUserScore);
                    }
                }
                else
                {
                    legalMove = Regex.IsMatch(expression, @"\b0*21+2+1*2*0*\b");
                    if (legalMove)
                    {
                        updateLine(i_Row, i_Col, directions[directionIndex], ref m_SecondUserScore, ref m_FirstUserScore);
                    }
                }

                directionIndex++;
            }
        }

        ////Updates specific line in the board after a Player's move
        private void updateLine(int i_Row, int i_Col, string i_Direction, ref int io_AddPlayerScore, ref int io_DecPlayerScore)
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
