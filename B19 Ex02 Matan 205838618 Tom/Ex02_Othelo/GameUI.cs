using System;

namespace Ex02_Othelo
{
    internal class GameUI
    {
        private string m_FirstUserName;
        private string m_SecondUserName;
        private Game m_Game = new Game();

        ////GameUI c'tor
        public GameUI()
        {
            Console.WriteLine("Welcome to Othelo!");
            initUserPreferences();
            startGame();
        }

        ////Takes user's preferences and assigns them to the logical class
        private void initUserPreferences()
        {
            Console.WriteLine("Please enter name for Player 1 and then press Enter: ");
            m_FirstUserName = Console.ReadLine();
            Console.WriteLine(
@"Would you like to play against another player, or against the computer?
Please press P for a game against another player, and C for playing against the computer.
After your choose, press Enter:");

            while (!m_Game.SetIsTwoPlayer(Console.ReadLine().ToLower()))
            {
                Console.WriteLine("Your input is invalid. Please try again, then press Enter");
            }

            if (m_Game.IsTwoPlayer)
            {
                Console.WriteLine("Please enter name for Player 2 and then press Enter: ");
                m_SecondUserName = Console.ReadLine();
            }
            else
            {
                m_SecondUserName = "Computer";
            }

            Console.WriteLine(
@"Please select the board size for the game.
For board size 6X6, please press button 6
For board size 8X8, please press button 8
After your choose, press Enter:");
            while (!m_Game.SetBoardSize(Console.ReadLine()))
            {
                Console.WriteLine("Your input is invalid. Please try again, then press Enter");
            }
        }

        ////Game starts 
        private void startGame()
        {
            m_Game.InitBoard();
            drawBoard(m_Game.BoardSize, m_Game.Board, m_Game.PlayerTurn, m_FirstUserName, m_SecondUserName, m_Game.FirstUserScore, m_Game.SecondUserScore);

            while (!m_Game.GameOver)
            {
                playTurn();                
            }

            endGame();
        }

        ////Takes a move from User, and assign it to the logical class
        ////If no moves are available for the user, the method asks the logical class to handle the issue
        private void playTurn()
        {
            string strFromUser = string.Empty;

            if (!m_Game.CheckAvailableMoves())
            {
                m_Game.HandleNoAvailableMoves();
            }
            else
            {
                if (!m_Game.IsComputerTurn())
                {
                    strFromUser = Console.ReadLine().ToUpper();
                    while (!m_Game.GetTurn(strFromUser) && !m_Game.IsUserWantToExit(strFromUser))
                    {
                        Console.WriteLine("illigal move. Please try again, then press Enter");
                        strFromUser = Console.ReadLine().ToUpper();
                    }

                    m_Game.CheckIfUserWantToExit(strFromUser);
                }
                else
                {
                    m_Game.GetTurn();
                }
            }

            drawBoard(m_Game.BoardSize, m_Game.Board, m_Game.PlayerTurn, m_FirstUserName, m_SecondUserName, m_Game.FirstUserScore, m_Game.SecondUserScore);          
            m_Game.CheckIfNoAvailableMovesForBothPlayers();
            if (m_Game.IsNoAvailableMovesForOnePlayer())
            {
                Console.WriteLine(string.Format(
@"No move is available for Player {0}, 
the turn goes to Player {1}",
m_Game.PlayerTurn + 1,
((m_Game.PlayerTurn + 1) % 2) + 1));
                m_Game.NextTurn();
                Console.Write(drawPossibleMoves()); 
            }
        }

        ////Checking which user won the game, and asks for re-game
        private void endGame()
        {
            if (m_Game.IsFirstPlayerWon())
            {
                Console.WriteLine("The Winner is " + m_FirstUserName);
            }
            else if (m_Game.IsSecondPlayerWon())
            {
                Console.WriteLine("The Winner is " + m_SecondUserName);
            }
            else
            {
                Console.WriteLine("The game ended in draw");
            }

            Console.WriteLine("Play again?[y/n] {default is n}");
            if (m_Game.IsPlayAgain(Console.ReadLine().ToLower()))
            {
                startGame();
            }
            else
            {
                Console.WriteLine("Thanks for playing!");
            }
        }

        ////Draw board to Console
        private void drawBoard(int i_BoardSize, int[,] i_Pieces, int i_PlayerTurn, string i_FirstUserName, string i_SecondUserName, int i_FirstUserScore, int i_SecondUserScore)
        {
            char colMark = 'A';
            int rowMark = 1;
            string board = "    ";

            Ex02.ConsoleUtils.Screen.Clear();
            for (int i = 0; i < i_BoardSize; i++)
            {
                board += colMark + "   ";
                colMark++;
            }

            board += Environment.NewLine + "   " + new string('=', 4 * i_BoardSize) + Environment.NewLine;
            for (int i = 0; i < i_BoardSize; i++)
            {
                board += rowMark.ToString() + " ";
                for (int j = 0; j < i_BoardSize; j++)
                {
                    switch (i_Pieces[i, j])
                    {
                        case 1:
                            board += "| O ";
                            break;
                        case 2:
                            board += "| X ";
                            break;
                        default:
                            board += "|   ";
                            break;
                    }
                }

                board += "|" + Environment.NewLine + "   " + new string('=', 4 * i_BoardSize) + Environment.NewLine;
                rowMark++;
            }

            board += "Player " + (i_PlayerTurn + 1) + " Turn" + Environment.NewLine;
            board += i_FirstUserName + ": " + i_FirstUserScore + " " + i_SecondUserName + ": " + i_SecondUserScore + Environment.NewLine;
            board += drawPossibleMoves();
            Console.Write(board);
        }

        ////Draw possible moves
        private string drawPossibleMoves()
        {
            string possibleMoves = "Please select a move from the possible moves, and then press enter:" + Environment.NewLine;

            if (m_Game.GameOver || !m_Game.AvailableMoveFirstPlayer || !m_Game.AvailableMoveSecondPlayer)
            {
                possibleMoves = string.Empty;
            }

            foreach (string move in m_Game.PossibleMoves)
            {
                possibleMoves += move + Environment.NewLine;
            }

            return possibleMoves;
        }
    }
}
