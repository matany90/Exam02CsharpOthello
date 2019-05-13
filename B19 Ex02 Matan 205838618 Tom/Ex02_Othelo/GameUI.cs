using System;

namespace Ex02_Othelo
{
    internal class GameUI
    {
        private Controller m_Controller = new Controller();

        public GameUI()
        {
            Console.WriteLine("Welcome to Othelo!");
            initUserPreferences();
            startGame();
        }

        private void initUserPreferences()
        {
            Console.WriteLine("Please enter name for Player 1 and then press Enter: ");
            m_Controller.SetFirstUserName(Console.ReadLine());
            Console.WriteLine(
@"Would you like to play against another player, or against the computer?
Please press P for a game against another player, and C for playing against the computer.
After your choose, press Enter:");
            while (!m_Controller.SetIsTwoPlayer(Console.ReadLine().ToLower()))
            {
                Console.WriteLine("Your input is invalid. Please try again, then press Enter");
            }

            if (m_Controller.GetIsTwoPlayer())
            {
                Console.WriteLine("Please enter name for Player 2 and then press Enter: ");
                m_Controller.SetSecondUserName(Console.ReadLine());
            }
            else
            {
                m_Controller.SetSecondUserName("Computer");
            }

            Console.WriteLine(
@"Please select the board size for the game.
For board size 6X6, please press button 6
For board size 8X8, please press button 8
After your choose, press Enter:");
            while (!m_Controller.SetBoardSize(Console.ReadLine()))
            {
                Console.WriteLine("Your input is invalid. Please try again, then press Enter");
            }
        }

        private void startGame()
        {
            m_Controller.InitBoard();
            drawBoard(m_Controller.GetBoardSize(), m_Controller.GetBoard(), m_Controller.GetPlayerTurn(), m_Controller.GetFirstUserName(), m_Controller.GetSecondUserName(), m_Controller.GetFirstUserScore(), m_Controller.GetSecondUserScore());
            while (!m_Controller.IsGameOver())
            {
                playTurn();
            }

            endGame();
        }

        private void playTurn()
        {
            string strFromUser = string.Empty;

            if (m_Controller.CheckAvailableMoves())
            {
                if (m_Controller.GetIsTwoPlayer() || (!m_Controller.GetIsTwoPlayer() && m_Controller.GetPlayerTurn() == 0))
                {
                    strFromUser = Console.ReadLine().ToUpper();
                    while (!m_Controller.GetTurn(strFromUser) && strFromUser != "Q")
                    {
                        Console.WriteLine("illigal move. Please try again, then press Enter");
                        strFromUser = Console.ReadLine().ToUpper();
                    }

                    if (strFromUser == "Q")
                    {
                        m_Controller.SetGameOver(true);
                    }
                }
                else
                {
                    m_Controller.GetTurn();
                }
            }
            else
            {
                if (m_Controller.GetFirstUserScore() + m_Controller.GetSecondUserScore() == m_Controller.GetBoardSize() * m_Controller.GetBoardSize())
                {
                    m_Controller.SetGameOver(true);
                }
                else
                {
                    if (m_Controller.GetPlayerTurn() == 0)
                    {
                        m_Controller.SetAvailableMoveFirstPlayer(false);
                    }
                    else
                    {
                        m_Controller.SetAvailableMoveSecondPlayer(false);
                    }
                }
            }

            drawBoard(m_Controller.GetBoardSize(), m_Controller.GetBoard(), m_Controller.GetPlayerTurn(), m_Controller.GetFirstUserName(), m_Controller.GetSecondUserName(), m_Controller.GetFirstUserScore(), m_Controller.GetSecondUserScore());
            if (!m_Controller.GetAvailableMoveFirstPlayer() && !m_Controller.GetAvailableMoveSecondPlayer())
            {
                m_Controller.SetGameOver(true);
            }
            else if (!m_Controller.GetAvailableMoveFirstPlayer() || !m_Controller.GetAvailableMoveSecondPlayer())
            {
                Console.WriteLine(string.Format(
@"No move is available for Player {0}, 
the turn goes to Player {1}",
m_Controller.GetPlayerTurn() + 1,
((m_Controller.GetPlayerTurn() + 1) % 2) + 1));
                m_Controller.NextTurn();
            }
        }

        private void endGame()
        {
            if (m_Controller.GetFirstUserScore() > m_Controller.GetSecondUserScore())
            {
                Console.WriteLine("The Winner is " + m_Controller.GetFirstUserName());
            }
            else if (m_Controller.GetFirstUserScore() < m_Controller.GetSecondUserScore())
            {
                Console.WriteLine("The Winner is " + m_Controller.GetSecondUserName());
            }
            else
            {
                Console.WriteLine("The game ended in draw");
            }

            Console.WriteLine("Play again?[y/n] {default is n}");
            if (m_Controller.IsPlayAgain(Console.ReadLine().ToLower()))
            {
                startGame();
            }
            else
            {
                Console.WriteLine("Thanks for playing!");
            }
        }

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
            board += i_FirstUserName + ":" + i_FirstUserScore + " " + i_SecondUserName + ":" + i_SecondUserScore + Environment.NewLine;
            Console.Write(board);
        }
    }
}
