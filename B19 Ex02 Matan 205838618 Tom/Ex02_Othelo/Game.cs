using System;

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
    }
}
